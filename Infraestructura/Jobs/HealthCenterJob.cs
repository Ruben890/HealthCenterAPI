using Hangfire;
using HealthCenterAPI.Contracts;
using HealthCenterAPI.Shared.Utils;

namespace HealthCenterAPI.Infraestructura.Jobs
{
    public class HealthCenterJob : IBackgroundJob
    {
        private readonly WebScrapingRIESS _webScrapingRIESS;

        public HealthCenterJob(WebScrapingRIESS webScrapingRIESS)
        {
            _webScrapingRIESS = webScrapingRIESS;
        }

        public async Task DownloadExcelFileIfNotExists()
        {
            DateTime? date = DateTime.Now;

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(directoryPath) || Directory.GetFiles(directoryPath).Length <= 0) date = null;
            await _webScrapingRIESS.DownloadExcelFile(date);
        }



        public void RegisterRecurringJobs()
        {
            RecurringJob.AddOrUpdate(
                         "download-excel-file",
                         () => DownloadExcelFileIfNotExists(),
                          Cron.Daily(1, 3));
        }
    }
}
