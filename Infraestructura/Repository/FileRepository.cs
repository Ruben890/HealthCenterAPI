
using HealthCenterAPI.Contracts.IRepository;
using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;
using SpreadsheetLight;


namespace HealthCenterAPI.Infraestructura.Repository
{
    public class FileRepository : IFileRepository
    {
        public List<HealthCenterDto> FilterHealthCenters(List<HealthCenterDto> healthCenters, GenericParameters parameters)
        {
            var query = healthCenters.AsQueryable();

            // Filtrar por provincia
            if (!string.IsNullOrEmpty(parameters.Province))
            {
                query = query.Where(hc => hc.Location!.Provincia!.Contains(parameters.Province, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por municipio
            if (!string.IsNullOrEmpty(parameters.Municipality))
            {
                query = query.Where(hc => hc.Location!.Municipio!.Contains(parameters.Municipality, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por sector
            if (!string.IsNullOrEmpty(parameters.Sector))
            {
                query = query.Where(hc => hc.Location!.Sector!.Contains(parameters.Sector, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por nivel de atención
            if (!string.IsNullOrEmpty(parameters.Level))
            {
                query = query.Where(hc => hc.Nivel_atencion!.Contains(parameters.Level, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por tipo de centro
            if (!string.IsNullOrEmpty(parameters.TypeCenter))
            {
                query = query.Where(hc => hc.Tipo_Centro_Primer_Nivel!.Contains(parameters.TypeCenter, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por área
            if (!string.IsNullOrEmpty(parameters.Area))
            {
                query = query.Where(hc => hc.Location!.Zona!.Contains(parameters.Area, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por servicios disponibles
            if (parameters.isOffices.HasValue)
            {
                query = query.Where(hc => hc.Services!.PNA_Consultorios == parameters.isOffices);
            }

            if (parameters.isDentistry.HasValue)
            {
                query = query.Where(hc => hc.Services!.PNA_Modulos_Odontologia == parameters.isDentistry);
            }

            if (parameters.isEmergency.HasValue)
            {
                query = query.Where(hc => hc.Services!.PNA_Emergencia == parameters.isEmergency);
            }

            if (parameters.isLaboratory.HasValue)
            {
                query = query.Where(hc => hc.Services!.PNA_Laboratorio == parameters.isLaboratory);
            }

            if (parameters.isSonography.HasValue)
            {
                query = query.Where(hc => hc.Services!.PNA_Sonografia == parameters.isSonography);
            }

            if (parameters.isPhysiotherapy.HasValue)
            {
                query = query.Where(hc => hc.Services!.PNA_Fisioterapia == parameters.isPhysiotherapy);
            }

            if (parameters.isInternet.HasValue)
            {
                query = query.Where(hc => hc.Services!.PNA_Internet == parameters.isInternet);
            }

            if (parameters.Xray.HasValue)
            {
                query = query.Where(hc => hc.Services!.PNA_Rayox_X == parameters.Xray);
            }

            // Devolver los resultados filtrados
            return query.ToList();
        }

        public async Task<List<HealthCenterDto>> MapExcelToPagedDto(GenericParameters parameters)
        {
            var healthCenters = new List<HealthCenterDto>();
            var file = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Files")).FirstOrDefault();

            try
            {
                await Task.Run(() =>
                {
                    using (var sl = new SLDocument(file))
                    {
                        var rows = sl.GetWorksheetStatistics().EndRowIndex; // Obtener el número total de filas
                        var batchSize = 500;

                        for (int i = 2; i <= rows; i += batchSize) // Comenzar desde la fila 2 para saltar el encabezado
                        {
                            var batchResults = new List<HealthCenterDto>();

                            for (int j = i; j < i + batchSize && j <= rows; j++)
                            {
                                var healthCenter = new HealthCenterDto
                                {
                                    NombreCentro = sl.GetCellValueAsString(j, 2), // Columna B
                                    Nivel_atencion = sl.GetCellValueAsString(j, 5), // Columna E
                                    Tipo_Centro_Primer_Nivel = sl.GetCellValueAsString(j, 11), // Columna K
                                    SRS = sl.GetCellValueAsString(j, 12), // Columna L
                                    TelCentro = sl.GetCellValueAsString(j, 40), // Columna AN
                                    RNC = sl.GetCellValueAsString(j, 41), // Columna AO
                                    Email = sl.GetCellValueAsString(j, 43), // Columna AQ
                                    FaxCentro = sl.GetCellValueAsString(j, 42), // Columna AP
                                    Anio_Apertura = sl.GetCellValueAsInt32(j, 35), // Columna AI
                                    Anio_Ultima_Ampl_Remod = sl.GetCellValueAsInt32(j, 36), // Columna AJ
                                    Administrada_Por = sl.GetCellValueAsString(j, 37), // Columna AK
                                    Complejidad_Servicio = sl.GetCellValueAsString(j, 9), // Columna I
                                    Location = new LocationDto
                                    {
                                        Provincia = sl.GetCellValueAsString(j, 18), // Columna R
                                        Municipio = sl.GetCellValueAsString(j, 20), // Columna T
                                        Distrito_Municipal = sl.GetCellValueAsString(j, 21), // Columna U
                                        Sector = sl.GetCellValueAsString(j, 30), // Columna AD
                                        DireccionCentro = sl.GetCellValueAsString(j, 3), // Columna C
                                        Barrio = sl.GetCellValueAsString(j, 23), // Columna W
                                        Sub_Barrio = sl.GetCellValueAsString(j, 24), // Columna X
                                        Gerencia_Area = sl.GetCellValueAsString(j, 26), // Columna Z
                                        Zona = sl.GetCellValueAsString(j, 28), // Columna AB
                                        LatCentro = sl.GetCellValueAsDouble(j, 49), // Columna AW
                                        LonCentro = sl.GetCellValueAsDouble(j, 50) // Columna AX
                                    },
                                    Services = new ServicesDto
                                    {
                                        PNA_Consultorios = sl.GetCellValueAsInt32(j, 54) > 0, // Columna BB
                                        PNA_Modulos_Odontologia = sl.GetCellValueAsInt32(j, 55) > 0, // Columna BC
                                        PNA_Emergencia = sl.GetCellValueAsInt32(j, 56) > 0, // Columna BD
                                        PNA_Laboratorio = sl.GetCellValueAsInt32(j, 57) > 0, // Columna DE
                                        PNA_Sonografia = sl.GetCellValueAsInt32(j, 58) > 0, // Columna BF
                                        PNA_Fisioterapia = sl.GetCellValueAsInt32(j, 59) > 0, // Columna BG
                                        PNA_Internet = sl.GetCellValueAsInt32(j, 61) > 0, // Columna BI
                                        PNA_Rayox_X = sl.GetCellValueAsInt32(j, 62) > 0 // Columna BJ
                                    }
                                };

                                batchResults.Add(healthCenter);
                            }

                            lock (healthCenters)
                            {
                                healthCenters.AddRange(batchResults);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al procesar el archivo Excel: {ex.Message}", ex);
            }

            return healthCenters;
        }



    }
}
