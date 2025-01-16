using AutoMapper;
using Hangfire;
using HealthCenterAPI.Contracts;
using HealthCenterAPI.Contracts.IRepository;
using HealthCenterAPI.Shared.Utils;

namespace HealthCenterAPI.Infraestructura.Jobs
{
    public class HealthCenterJob : IBackgroundJob
    {
        private readonly WebScrapingRIESS _webScrapingRIESS;
        private readonly IFileRepository _fileRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public HealthCenterJob
            (WebScrapingRIESS webScrapingRIESS,
            IFileRepository fileRepository,
            IConfiguration configuration,
            IMapper mapper)
        {
            _webScrapingRIESS = webScrapingRIESS;
            _fileRepository = fileRepository;
            _config = configuration;
            _mapper = mapper;
        }

        public async Task DownloadExcelFileIfNotExists()
        {
            DateTime? date = DateTime.Now;

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(directoryPath) || Directory.GetFiles(directoryPath).Length <= 0) date = null;
            await _webScrapingRIESS.DownloadExcelFile(date);
        }

        public async Task MapFromExelToDB()
        {

        }


        public void RegisterRecurringJobs()
        {
            RecurringJob.AddOrUpdate(
                         "download-excel-file",
                         () => DownloadExcelFileIfNotExists(),
                          Cron.Daily(1, 3));

            if (_config.GetValue<bool>("DataSourceType:Database", false))
            {
                RecurringJob.AddOrUpdate(
                                     "map-from-exel-to-db",
                                     () => MapFromExelToDB(),
                                      Cron.Daily(1, 3));
            }
        }
    }
}
