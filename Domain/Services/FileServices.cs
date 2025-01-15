using HealthCenterAPI.Contracts.Iservices;
using HealthCenterAPI.Contracts.IServices;
using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;
using HealthCenterAPI.Shared.Utils;

namespace HealthCenterAPI.Domain.Services
{
    public class FileServices : IFileServices
    {
        private readonly WebScrapingRIESS _webScrapingRIESS;

        public FileServices(WebScrapingRIESS webScrapingRIESS)
        {
            _webScrapingRIESS = webScrapingRIESS;
        }



        public async Task<IEnumerable<HealthCenterDto>> GetHealthCenter(GenericParameters parameters)
        {
            try
            {
                await EnsureExcelFileExistsAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task EnsureExcelFileExistsAsync()
        {
            DateTime? date = DateTime.Now;

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "files");
            if (!Directory.Exists(directoryPath) || Directory.GetFiles(directoryPath).Length <= 0) date = null;
            await _webScrapingRIESS.DownloadExcelFile(date);
        }

    }
}
