using AutoMapper;
using HealthCenterAPI.Domain.Entity;
using HealthCenterAPI.Shared.Dto;

namespace HealthCenterAPI.Presentation.Mapping
{
    public class HealthCenterProfile : MappingProfile
    {
        public HealthCenterProfile()
        {
            CreateMap<HealthCenterDto, HealthCenter>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NombreCentro))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Nivel_atencion))
                .ForMember(dest => dest.TypeCenter, opt => opt.MapFrom(src => src.Tipo_Centro_Primer_Nivel))
                .ForMember(dest => dest.SRS, opt => opt.MapFrom(src => src.SRS))
                .ForMember(dest => dest.Tel, opt => opt.MapFrom(src => src.TelCentro))
                .ForMember(dest => dest.RNC, opt => opt.MapFrom(src => src.RNC))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Fax, opt => opt.MapFrom(src => src.FaxCentro))
                .ForMember(dest => dest.OpeningYear, opt => opt.MapFrom(src => src.Anio_Apertura))
                .ForMember(dest => dest.lastRenovationYear, opt => opt.MapFrom(src => src.Anio_Ultima_Ampl_Remod))
                .ForMember(dest => dest.Managed_By, opt => opt.MapFrom(src => src.Administrada_Por))
                .ForMember(dest => dest.ServiceComplexity, opt => opt.MapFrom(src => src.Complejidad_Servicio))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Services));
        }
    }
}
