using AutoMapper;
using HealthCenterAPI.Domain.Entity;
using HealthCenterAPI.Shared.Dto;
using NetTopologySuite.Geometries;

namespace HealthCenterAPI.Presentation.Mapping
{
    public class LocationProfiles : MappingProfile
    {
        public LocationProfiles()
        {
            CreateMap<LocationDto, Domain.Entity.Location>()
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Provincia))
                .ForMember(dest => dest.Municipality, opt => opt.MapFrom(src => src.Municipio))
                .ForMember(dest => dest.MunicipalDistrict, opt => opt.MapFrom(src => src.Distrito_Municipal))
                .ForMember(dest => dest.Sector, opt => opt.MapFrom(src => src.Sector))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.DireccionCentro))
                .ForMember(dest => dest.Neighborhood, opt => opt.MapFrom(src => src.Barrio))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Gerencia_Area))
                .ForMember(dest => dest.Zone, opt => opt.MapFrom(src => src.Zona))
                .ForMember(dest => dest.Ubication, opt => opt.MapFrom(src =>
                    geometryFactory.CreatePoint(new Coordinate(src.LonCentro, src.LatCentro))));

            CreateMap<Domain.Entity.Location, LocationDto>()
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Province))
                .ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => src.Municipality))
                .ForMember(dest => dest.Distrito_Municipal, opt => opt.MapFrom(src => src.MunicipalDistrict))
                .ForMember(dest => dest.Sector, opt => opt.MapFrom(src => src.Sector))
                .ForMember(dest => dest.DireccionCentro, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Barrio, opt => opt.MapFrom(src => src.Neighborhood))
                .ForMember(dest => dest.Gerencia_Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dest => dest.Zona, opt => opt.MapFrom(src => src.Zone))
                .ForMember(dest => dest.LatCentro, opt => opt.MapFrom(src => src.Ubication != null ? src.Ubication.Y : default))
                .ForMember(dest => dest.LonCentro, opt => opt.MapFrom(src => src.Ubication != null ? src.Ubication.X : default));
        }
    }
}
