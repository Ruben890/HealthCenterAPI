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


        public void RegisterRecurringJobs()
        {
            throw new NotImplementedException();
        }
    }
}
