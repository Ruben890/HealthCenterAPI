using Newtonsoft.Json;
using System.Text.Json;


namespace HealthCenterAPI.Shared.Dto
{
    public class HealthCenterDto
    {
        [JsonProperty("Name")]      
        public string? NombreCentro { get; set; }

        [JsonProperty("Level")]
        public string? Nivel_atencion { get; set; }

        [JsonProperty("TypeCenter")]
        public string? Tipo_Centro_Primer_Nivel { get; set; }

        public string? SRS { get; set; } = null!;

        [JsonProperty("Tel")]
        public string? TelCentro { get; set; } = null!;
        public string? RNC { get; set; } = null!;
        public string Email { get; set; } = null!;

        [JsonProperty("Fax")]
        public string FaxCentro { get; set; } = null!;

        [JsonProperty("OpeningYear")]
        public int? Anio_Apertura { get; set; }

        [JsonProperty("lastRenovationYear")]
        public int? Anio_Ultima_Ampl_Remod { get; set; } = null!;

        [JsonProperty("Managed_By")]
        public string? Administrada_Por { get; set; } = null!;

        [JsonProperty("ServiceComplexity")]
        public string? Complejidad_Servicio { get; set; } = null!;
        public virtual LocationDto? Location { get; set; } = null!;
        public virtual ServicesDto? Services { get; set; } = null!;
    }
}
