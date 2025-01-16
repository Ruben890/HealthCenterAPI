using HealthCenterAPI.Contracts.IRepository;
using HealthCenterAPI.Contracts.Iservices;
using HealthCenterAPI.Contracts.IServices;
using HealthCenterAPI.Domain.Entity;
using HealthCenterAPI.Shared;
using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;
using HealthCenterAPI.Shared.RequestFeatures;
using HealthCenterAPI.Shared.Utils;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;

namespace HealthCenterAPI.Domain.Services
{
    public class FileServices : IFileServices
    {
        private readonly WebScrapingRIESS _webScrapingRIESS;
        private readonly IMemoryCache _cache;
        private IFileRepository _fileRepository;
        public FileServices(IMemoryCache cache, WebScrapingRIESS webScrapingRIESS, IFileRepository fileRepository)
        {
            _cache = cache;
            _webScrapingRIESS = webScrapingRIESS;
            _fileRepository = fileRepository;
        }

        public async Task<BaseResponse> GetHealthCenter(GenericParameters parameters)
        {
            var response = new BaseResponse();

            try
            {
                // Generar una clave única basada en los parámetros
                string cacheKey = GenerateCacheKey(parameters);

                // Intentar obtener los datos del caché
                if (!_cache.TryGetValue(cacheKey, out PagedList<HealthCenterDto> healthCenter))
                {
                    // Verificar si el archivo Excel existe
                    await EnsureExcelFileExistsAsync();

                    // Mapear los datos del archivo Excel a una lista de DTOs
                    var healthCenterList = await _fileRepository.MapExcelToPagedDto(parameters);

                    // Filtrar y paginar los centros de salud
                    var filteredHealthCenters = _fileRepository.FilterHealthCenters(healthCenterList, parameters);
                    healthCenter = PagedList<HealthCenterDto>.ToPagedList(filteredHealthCenters, parameters.PageNumber, parameters.PageSize);
                   
                    // Configurar las opciones de caché y almacenar los datos
                    _cache.Set(cacheKey, healthCenter, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                    });
                }

                response.SetPagination(healthCenter!.Pagination);
                response.Details = healthCenter;
            }
            catch (Exception)
            {
                throw;
            }

            return response;
        }


        private string GenerateCacheKey(GenericParameters parameters)
        {
            var json = JsonConvert.SerializeObject(parameters);
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
            return $"HealthCenters_{Convert.ToBase64String(hashBytes)}";
        }

        private async Task EnsureExcelFileExistsAsync()
        {
            DateTime? date = DateTime.Now;

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(directoryPath) || Directory.GetFiles(directoryPath).Length <= 0) date = null;
            await _webScrapingRIESS.DownloadExcelFile(date);
        }

    }
}
