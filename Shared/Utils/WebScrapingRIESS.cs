using ScrapySharp.Network;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ScrapySharp.Extensions;
using System.Globalization;
using System.Text.RegularExpressions;

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
                var fileLink = _page.Html.CssSelect("div.file a.wpfd_downloadlink")
                                 .FirstOrDefault()?.Attributes["href"]?.Value;

                if (string.IsNullOrWhiteSpace(fileLink))
                {
                    throw new InvalidOperationException("No se pudo encontrar el enlace de descarga del archivo Excel.");
                }

                var fileName = _page.Html.CssSelect("div.file h3").FirstOrDefault()?.InnerText.Trim();
                var fileDateStr = _page.Html.CssSelect("div.file div.file-dated").FirstOrDefault()?.InnerText;

                if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(fileDateStr))
                {
                    throw new InvalidOperationException("No se pudo encontrar el nombre del archivo o la fecha en la página.");
                }

                var fileDate = DateTime.ParseExact(fileDateStr.Split(':')[1].Trim(), "dd-MM-yyyy", null).ToString("yyyy-MM-dd");

                fileName = ProcessFileName(fileName);

                if (date.HasValue && (DateTime.Parse(fileDate) < date.Value))
                {
                    Console.WriteLine("La fecha del archivo es menor a la fecha del documento. No se descargará.");
                    return;
                }

                var downloadDate = DateTime.Now.ToString("yyyy-MM-dd");
                var outputFilename = $"{fileName}_{fileDate}_{downloadDate}{Path.GetExtension(fileLink)}";
                var filePath = await DownloadAndSaveFile(fileLink, outputFilename);

                CleanOldFiles(filePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al descargar el archivo: {ex.Message}");
                throw;
            }
        }

        private string ProcessFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return string.Empty;

            fileName = Regex.Replace(fileName, @"\d", string.Empty) // Eliminar números
                           .Replace("--", string.Empty)           // Eliminar guiones dobles
                           .Trim();                               // Eliminar espacios adicionales

            return string.Concat(Regex.Split(fileName, @"\s+")
                                       .Select((word, index) =>
                                           index == 0
                                               ? word.ToLowerInvariant()
                                               : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(word.ToLowerInvariant()))); // CamelCase
        }

        private async Task<string> DownloadAndSaveFile(string fileLink, string outputFilename)
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, outputFilename);

            using (var client = new HttpClient())
            {
                var fileResponse = await client.GetAsync(fileLink);
                fileResponse.EnsureSuccessStatusCode();

                var fileBytes = await fileResponse.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(filePath, fileBytes);
            }

            Console.WriteLine($"Archivo descargado y guardado como: {filePath}");
            return filePath;
        }

        private void CleanOldFiles(string newFilePath)
        {
            var directoryPath = Path.GetDirectoryName(newFilePath);
            var existingFiles = Directory.GetFiles(directoryPath!).Where(f => f != newFilePath);

            foreach (var existingFile in existingFiles)
            {
                File.Delete(existingFile);
                Console.WriteLine($"Archivo anterior eliminado: {existingFile}");
            }
        }

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
