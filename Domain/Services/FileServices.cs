using HealthCenterAPI.Contracts.IRepository;
using HealthCenterAPI.Contracts.Iservices;
using HealthCenterAPI.Contracts.IServices;
using HealthCenterAPI.Domain.Entity;
using HealthCenterAPI.Shared;
using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;
using HealthCenterAPI.Shared.RequestFeatures;
using HealthCenterAPI.Shared.Utils;
using Newtonsoft.Json;

namespace HealthCenterAPI.Domain.Services
{
    public class FileServices : IFileServices
    {
        private readonly WebScrapingRIESS _webScrapingRIESS;
        private IFileRepository _fileRepository;
        public FileServices(WebScrapingRIESS webScrapingRIESS, IFileRepository fileRepository)
        {
            _webScrapingRIESS = webScrapingRIESS;
            _fileRepository = fileRepository;
        }



        public async Task<BaseResponse> GetHealthCenter(GenericParameters parameters)
        {
            var response = new BaseResponse();

            await EnsureExcelFileExistsAsync();
            var list = await _fileRepository.MapExcelToPagedDto(parameters);

            var healthCenter = PagedList<HealthCenterDto>.ToPagedList((_fileRepository.FilterHealthCenters(list, parameters)), parameters.PageNumber, parameters.PageSize); ;
            response.SetPagination(healthCenter.Pagination);
            response.Details = healthCenter;

            return response;

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
