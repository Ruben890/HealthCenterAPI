﻿using AutoMapper;
using NetTopologySuite.Geometries;
using NetTopologySuite;

namespace HealthCenterAPI.Mapping
{
    public class MappingProfile : Profile
    {
        protected readonly GeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        public MappingProfile() { }
        
        
    }
}
