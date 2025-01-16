using HealthCenterAPI.Contracts.IRepository;
using HealthCenterAPI.Domain.Entity;
using HealthCenterAPI.Repository;
using HealthCenterAPI.Shared.Dto;
using HealthCenterAPI.Shared.QueryParameters;
using HealthCenterAPI.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace HealthCenterAPI.Infraestructura.Repository
{
    public class HealthCenterRepository : IHealthCenterRepository
    {

        private readonly HealthCenterContex _context;

        public HealthCenterRepository(HealthCenterContex contex)
        {
            _context = contex;
        }


        private IQueryable<HealthCenter> FilterHealthCenters(IQueryable<HealthCenter> healthCenters, GenericParameters parameters)
        {
            var query = healthCenters;

            // Filtrar por provincia
            if (!string.IsNullOrEmpty(parameters.Province))
            {
                query = query.Where(hc => hc.Location!.Province!.Contains(parameters.Province, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por municipio
            if (!string.IsNullOrEmpty(parameters.Municipality))
            {
                query = query.Where(hc => hc.Location!.Municipality!.Contains(parameters.Municipality, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por sector
            if (!string.IsNullOrEmpty(parameters.Sector))
            {
                query = query.Where(hc => hc.Location!.Sector!.Contains(parameters.Sector, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por nivel de atención
            if (!string.IsNullOrEmpty(parameters.Level))
            {
                query = query.Where(hc => hc.Level!.Contains(parameters.Level, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por tipo de centro
            if (!string.IsNullOrEmpty(parameters.TypeCenter))
            {
                query = query.Where(hc => hc.TypeCenter!.Contains(parameters.TypeCenter, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por área
            if (!string.IsNullOrEmpty(parameters.Area))
            {
                query = query.Where(hc => hc.Location!.Area!.Contains(parameters.Area, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por servicios disponibles
            if (parameters.isOffices.HasValue)
            {
                query = query.Where(hc => hc.Services!.isOffices == parameters.isOffices);
            }

            if (parameters.isDentistry.HasValue)
            {
                query = query.Where(hc => hc.Services!.isDentistry == parameters.isDentistry);
            }

            if (parameters.isEmergency.HasValue)
            {
                query = query.Where(hc => hc.Services!.isEmergency == parameters.isEmergency);
            }

            if (parameters.isLaboratory.HasValue)
            {
                query = query.Where(hc => hc.Services!.isLaboratory == parameters.isLaboratory);
            }

            if (parameters.isSonography.HasValue)
            {
                query = query.Where(hc => hc.Services!.isSonography == parameters.isSonography);
            }

            if (parameters.isPhysiotherapy.HasValue)
            {
                query = query.Where(hc => hc.Services!.isPhysiotherapy == parameters.isPhysiotherapy);
            }

            if (parameters.isInternet.HasValue)
            {
                query = query.Where(hc => hc.Services!.isInternet == parameters.isInternet);
            }

            if (parameters.Xray.HasValue)
            {
                query = query.Where(hc => hc.Services!.Xray == parameters.Xray);
            }

            // Devolver los resultados filtrados
            return query;
        }

        public async Task<PagedList<HealthCenterDto>> GetAllHealthCenter(GenericParameters parameters)
        {
            ;
            var query = _context.HealthCenters
                            .AsNoTracking()
                            .AsQueryable()
                            .AsSplitQuery();

            FilterHealthCenters(query, parameters);

            var result = query.Select(x => new HealthCenterDto
            {
                NombreCentro = x.Name,
                Administrada_Por = x.Managed_By,
                Anio_Apertura = x.OpeningYear,
                Anio_Ultima_Ampl_Remod = x.lastRenovationYear,
                Complejidad_Servicio = x.ServiceComplexity,
                Email = x.Email,
                TelCentro = x.Tel,
                FaxCentro = x.Fax,
                RNC = x.RNC,
                SRS = x.SRS,
                Nivel_atencion = x.Level,
                Tipo_Centro_Primer_Nivel = x.Level,
                Location = new LocationDto
                {
                    DireccionCentro = x.Location!.Direction,
                    Provincia = x.Location.Province,
                    Distrito_Municipal = x.Location.MunicipalDistrict,
                    Municipio = x.Location.Municipality,
                    Barrio = x.Location.Neighborhood,
                    Gerencia_Area = x.Location.Area,
                    Sector = x.Location.Sector,
                    Zona = x.Location.Zone,
                    LonCentro = x.Location.Ubication!.X,
                    LatCentro = x.Location.Ubication.Y,
                },
                Services = new ServicesDto
                {
                    PNA_Consultorios = x.Services.isOffices,
                    PNA_Emergencia = x.Services.isEmergency,
                    PNA_Fisioterapia = x.Services.isPhysiotherapy,
                    PNA_Laboratorio = x.Services.isLaboratory,
                    PNA_Modulos_Odontologia = x.Services.isLaboratory,
                    PNA_Rayox_X = x.Services.Xray,
                    PNA_Sonografia = x.Services.isSonography,
                    PNA_Internet = x.Services.isInternet
                }

            });

            return await PagedList<HealthCenterDto>.ToPagedListAsync(result, parameters.PageNumber, parameters.PageSize);

        }
    }
}
