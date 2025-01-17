using Hangfire;
using HealthCenterAPI.Domain.Contracts;
using HealthCenterAPI.Infraestructura.Jobs;

namespace HealthCenterAPI.Extencion
{
    public static class BackgroundJobConfigure
    {

        public static void ConfigureBackgroundJobs(this IServiceCollection services)
        {
            // Registrar los trabajos que implementan IBackgroundJob
            services.AddScoped<IBackgroundJob, HealthCenterJob>();
            services.AddSingleton<IHostedService, BackgroundJobService>();
        }

        public class BackgroundJobService : IHostedService
        {
            private readonly IServiceProvider _serviceProvider;

            public BackgroundJobService(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public Task StartAsync(CancellationToken cancellationToken)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobs = scope.ServiceProvider.GetServices<IBackgroundJob>();
                    foreach (var job in jobs)
                    {
                        job.RegisterRecurringJobs();
                    }
                }

                return Task.CompletedTask;
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                // Aquí puedes agregar lógica para limpiar trabajos si es necesario
                return Task.CompletedTask;
            }

        }
    }
}
