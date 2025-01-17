using AutoMapper;
using Hangfire;
using HealthCenterAPI.Domain.Contracts;
using HealthCenterAPI.Domain.Contracts.IRepository;
using HealthCenterAPI.Domain.Entity;
using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;
using HealthCenterAPI.Shared.Utils;

namespace HealthCenterAPI.Infraestructura.Jobs
{
    public class HealthCenterJob : IBackgroundJob
    {
        private readonly WebScrapingRIESS _webScrapingRIESS;
        private readonly IFileRepository _fileRepository;
        private readonly IHealthCenterRepository _healthCenterRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public HealthCenterJob
            (WebScrapingRIESS webScrapingRIESS,
            IFileRepository fileRepository,
            IConfiguration configuration,
            IHealthCenterRepository healthCenterRepository,
            IMapper mapper)
        {
            _webScrapingRIESS = webScrapingRIESS;
            _fileRepository = fileRepository;
            _config = configuration;
            _mapper = mapper;
            _healthCenterRepository = healthCenterRepository;
        }

        public async Task DownloadExcelFileIfNotExists()
        {
            DateTime? date = DateTime.Now;

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(directoryPath) || Directory.GetFiles(directoryPath).Length <= 0) date = null;
            await _webScrapingRIESS.DownloadExcelFile(date);
        }

        public async Task MapExcelDataToDatabaseAsync()
        {
            var healthCenters = _fileRepository.MapExcelToPagedDto() as IAsyncEnumerable<HealthCenterDto>;

            if (healthCenters == null)
            {
                throw new InvalidOperationException("Mapping Excel data to DTO failed.");
            }

            // Procesar datos en lotes para evitar múltiples enumeraciones y exceso de memoria
            const int batchSize = 500; // Tamaño del lote
            var batch = new List<HealthCenterDto>();

            await foreach (var healthCenter in healthCenters)
            {
                batch.Add(healthCenter);

                if (batch.Count >= batchSize)
                {
                    await ProcessBatchAsync(batch);
                    batch.Clear();
                }
            }

            // Procesar el último lote si queda algún elemento
            if (batch.Any())
            {
                await ProcessBatchAsync(batch);
            }
        }

        private async Task ProcessBatchAsync(List<HealthCenterDto> batch)
        {
            // Obtener nombres existentes en la base de datos en un solo lote
            var existingNames = await _healthCenterRepository.GetExistingNamesAsync(
                batch.Select(h => h.NombreCentro).Distinct().ToList()!
            );

            // Filtrar datos para evitar procesar duplicados
            var newHealthCenters = batch
                .Where(h => !existingNames.Contains(h.NombreCentro!))
                .ToList();

            // Mapear entidades
            var healthCenterEntities = new List<HealthCenter>();
            var locationEntities = new List<Location>();
            var serviceEntities = new List<Services>();

            foreach (var healthCenterDto in newHealthCenters)
            {
                var healthCenter = _mapper.Map<HealthCenter>(healthCenterDto);
                healthCenterEntities.Add(healthCenter);

                var location = _mapper.Map<Location>(healthCenterDto.Location);
                location.HealthCenter = healthCenter;
                locationEntities.Add(location);

                var services = _mapper.Map<Services>(healthCenterDto.Services);
                services.HealthCenter = healthCenter;
                serviceEntities.Add(services);
            }

            // Insertar datos en lotes
            var tasks = new List<Task>();
            if (healthCenterEntities.Any())
            {
                tasks.Add(_healthCenterRepository.InserRangeAsync(healthCenterEntities));
            }

            if (locationEntities.Any())
            {
                tasks.Add(_healthCenterRepository.InserRangeAsync(locationEntities));
            }

            if (serviceEntities.Any())
            {
                tasks.Add(_healthCenterRepository.InserRangeAsync(serviceEntities));
            }

            await Task.WhenAll(tasks);
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
                     "MapExcelDataToDatabaseAsync",
                     () => MapExcelDataToDatabaseAsync(),
                     Cron.Weekly(DayOfWeek.Tuesday));

            }
        }
    }
}
