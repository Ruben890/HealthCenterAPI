using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthCenterAPI.Shared.Dto
{
    public class LocationDto
    {
        [JsonProperty("Province")]
        public string? Provincia { get; set; } = null!;

        [JsonProperty("Municipality")]
        public string? Municipio { get; set; } = null!;

        [JsonProperty("MunicipalDistrict")]
        public string? Distrito_Municipal { get; set; } = null!;

        [JsonProperty("Sector")]
        public string? Sector { get; set; } = null!;

        [JsonProperty("Address")]
        public string? DireccionCentro { get; set; } = null!;

        [JsonProperty("Neighborhood")]
        public string? Barrio { get; set; }

        [JsonProperty("SubNeighborhood")]
        public string? Sub_Barrio { get; set; }

        [JsonProperty("Area")]
        public string? Gerencia_Area { get; set; }

        [JsonProperty("Zone")]
        public string? Zona { get; set; }

        [JsonProperty("Latitud")]
        [Range(-90, 90)]
        public double LatCentro { get; set; }

        [JsonProperty("Longitud")]
        [Range(-180, 180)]
        public double LonCentro { get; set; }
    }
}
