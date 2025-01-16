using AutoMapper;
using HealthCenterAPI.Domain.Entity;
using HealthCenterAPI.Shared.Dto;

namespace HealthCenterAPI.Mapping
{
    public class ServicesProfiles : MappingProfile
    {
        public ServicesProfiles()
        {
            // Mapping from ServicesDto to Services
            CreateMap<ServicesDto, Services>()
                .ForMember(dest => dest.isOffices, opt => opt.MapFrom(src => src.PNA_Consultorios))
                .ForMember(dest => dest.isDentistry, opt => opt.MapFrom(src => src.PNA_Modulos_Odontologia))
                .ForMember(dest => dest.isEmergency, opt => opt.MapFrom(src => src.PNA_Emergencia))
                .ForMember(dest => dest.isLaboratory, opt => opt.MapFrom(src => src.PNA_Laboratorio))
                .ForMember(dest => dest.isSonography, opt => opt.MapFrom(src => src.PNA_Sonografia))
                .ForMember(dest => dest.isPhysiotherapy, opt => opt.MapFrom(src => src.PNA_Fisioterapia))
                .ForMember(dest => dest.isInternet, opt => opt.MapFrom(src => src.PNA_Internet))
                .ForMember(dest => dest.Xray, opt => opt.MapFrom(src => src.PNA_Rayox_X)).ReverseMap();


        }
    }
}
