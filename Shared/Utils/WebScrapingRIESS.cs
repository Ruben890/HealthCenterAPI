using ScrapySharp.Network;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ScrapySharp.Extensions;

namespace HealthCenterAPI.Shared.Utils
{
    public class WebScrapingRIESS
    {
        private readonly WebPage _page;
        private readonly IConfiguration _configuration;

        public WebScrapingRIESS(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var browser = new ScrapingBrowser();
            _page = browser.NavigateToPage(new Uri(GetConfigurationValue("WebUrl")));
        }

        public async Task DownloadExcelFile(DateTime? date = null)
        {
            try
            {
                // Obtener el enlace de descarga del archivo Excel
                var fileLink = _page.Html.CssSelect("div.file a.wpfd_downloadlink").FirstOrDefault()?.Attributes["href"]?.Value;
                if (string.IsNullOrWhiteSpace(fileLink))
                {
                    throw new InvalidOperationException("No se pudo encontrar el enlace de descarga del archivo Excel.");
                }

                // Obtener el nombre del archivo y la fecha del archivo
                var fileName = _page.Html.CssSelect("div.file h3").FirstOrDefault()?.InnerText.Trim();
                var fileDateStr = _page.Html.CssSelect("div.file div.file-dated span").FirstOrDefault()?.InnerText;
                if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(fileDateStr))
                {
                    throw new InvalidOperationException("No se pudo encontrar el nombre del archivo o la fecha en la página.");
                }

                // Extraer la fecha del archivo en formato "dd-MM-yyyy"
                var fileDate = DateTime.ParseExact(fileDateStr.Split(':')[1].Trim(), "dd-MM-yyyy", null).ToString("yyyy-MM-dd");

                // Verificar si la fecha del archivo es mayor que la fecha del documento
                if (date.HasValue && DateTime.Parse(fileDate) < date.Value)
                {
                    Console.WriteLine("La fecha del archivo es menor a la fecha del documento. No se descargará.");
                    return;
                }

                // Obtener la fecha de la descarga
                var downloadDate = DateTime.Now.ToString("yyyy-MM-dd");

                // Descargar el archivo Excel
                using (var client = new HttpClient())
                {
                    var fileResponse = await client.GetAsync(fileLink);
                    fileResponse.EnsureSuccessStatusCode();

                    // Obtener la extensión del archivo
                    var fileExtension = Path.GetExtension(fileLink);

                    // Crear el nombre del archivo que se guardará
                    var outputFilename = $"{fileName}_{fileDate}_{downloadDate}{fileExtension}";

                    // Crear el directorio "files" si no existe
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "files");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Definir la ruta completa para el archivo
                    var filePath = Path.Combine(directoryPath, outputFilename);

                    // Descargar y guardar el nuevo archivo en disco
                    var fileBytes = await fileResponse.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(filePath, fileBytes);

                    Console.WriteLine($"Archivo descargado y guardado como: {filePath}");

                    // Eliminar el archivo anterior si existe
                    var existingFiles = Directory.GetFiles(directoryPath).Where(f => f != filePath);
                    foreach (var existingFile in existingFiles)
                    {
                        File.Delete(existingFile);
                        Console.WriteLine($"Archivo anterior eliminado: {existingFile}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.Error.WriteLine($"Error al descargar el archivo: {ex.Message}");
                throw;
            }
        }


        // Método privado para obtener valores de configuración con validación
        private string GetConfigurationValue(string key)
        {
            var value = _configuration[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"El valor '{key}' en la configuración no puede ser nulo o vacío.");
            }

            return value;
        }
    }
}
