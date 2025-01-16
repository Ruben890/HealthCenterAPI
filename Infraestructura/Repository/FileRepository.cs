using ClosedXML.Excel;
using HealthCenterAPI.Contracts.IRepository;
using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;
using HealthCenterAPI.Shared.RequestFeatures;

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
                    using (var workbook = new XLWorkbook(file))
                    {
                        var worksheet = workbook.Worksheet(1); // Asumiendo que los datos están en la primera hoja
                        var rows = worksheet.RowsUsed().Skip(1); // Saltar encabezados

                        foreach (var row in rows)
                        {
                            var healthCenter = new HealthCenterDto
                            {
                                NombreCentro = row.Cell("B").GetString(),
                                Nivel_atencion = row.Cell("E").GetString(),
                                Tipo_Centro_Primer_Nivel = row.Cell("K").GetString(),
                                SRS = row.Cell("L").GetString(),
                                TelCentro = row.Cell("AN").GetString(),
                                RNC = row.Cell("AO").GetString(),
                                Email = row.Cell("AQ").GetString(),
                                FaxCentro = row.Cell("AP").GetString(),
                                Anio_Apertura = row.Cell("AI").GetValue<int?>(),
                                Anio_Ultima_Ampl_Remod = row.Cell("AJ").GetValue<int?>(),
                                Administrada_Por = row.Cell("AK").GetString(),
                                Complejidad_Servicio = row.Cell("I").GetValue<string>(),
                                Location = new LocationDto
                                {
                                    Provincia = row.Cell("R").GetString(),
                                    Municipio = row.Cell("T").GetString(),
                                    Distrito_Municipal = row.Cell("U").GetString(),
                                    Sector = row.Cell("AD").GetString(),
                                    DireccionCentro = row.Cell("C").GetString(),
                                    Barrio = row.Cell("W").GetString(),
                                    Sub_Barrio = row.Cell("X").GetString(),
                                    Gerencia_Area = row.Cell("Z").GetString(),
                                    Zona = row.Cell("AB").GetString(),
                                    LatCentro = row.Cell("AW").GetValue<double>(),
                                    LonCentro = row.Cell("AX").GetValue<double>()
                                },
                                Services = new ServicesDto
                                {
                                    PNA_Consultorios = row.Cell("BB").GetValue<int?>() <= 0 ? false : true,
                                    PNA_Modulos_Odontologia = row.Cell("BC").GetValue<int?>() <= 0 ? false : true,
                                    PNA_Emergencia = row.Cell("BD").GetValue<int?>() <= 0 ? false : true,
                                    PNA_Laboratorio = row.Cell("DE").GetValue<int?>() <= 0 ? false : true,
                                    PNA_Sonografia = row.Cell("BF").GetValue<int?>() <= 0 ? false : true,
                                    PNA_Fisioterapia = row.Cell("BG").GetValue<int?>() <= 0 ? false : true,
                                    PNA_Internet = row.Cell("BI").GetValue<int?>() <= 0 ? false : true,
                                    PNA_Rayox_X = row.Cell("BJ").GetValue<int?>() <= 0 ? false : true
                                }
                            };


                            healthCenters.Add(healthCenter);
                        }
                    }
                });
            }
            catch (Exception)
            {

                throw;
            }

            return healthCenters;
        }


    }
}
