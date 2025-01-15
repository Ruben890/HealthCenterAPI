using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthCenterAPI.Shared.Dto
{
    public class ServicesDto
    {

        [JsonProperty("isOffices")]
        public bool? PNA_Consultorios { get; set; } = null!;

        [JsonProperty("isDentistry")]
        public bool? PNA_Modulos_Odontologia { get; set; } = null!;

        [JsonProperty("isEmergency")]
        public bool? PNA_Emergencia { get; set; } = null!;

        [JsonProperty("isLaboratory")]
        public bool? PNA_Laboratorio { get; set; } = null!;

        [JsonProperty("isSonography")]
        public bool? PNA_Sonografia { get; set; } = null!;

        [JsonProperty("isPhysiotherapy")]
        public bool? PNA_Fisioterapia { get; set; } = null!;

        [JsonProperty("isInternet")]
        public bool? PNA_Internet { get; set; } = null!;

        [JsonProperty("Xray")]
        public bool? PNA_Rayox_X { get; set; } = null!;
    }
}
