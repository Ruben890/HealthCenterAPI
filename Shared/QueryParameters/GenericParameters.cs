using DocumentFormat.OpenXml.Presentation;
using HealthCenterAPI.Shared.RequestFeatures;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace HealthCenterAPI.Shared.QueryParameters
{
    public class GenericParameters : RequestParameters
    {

        /// <summary>
        /// Especifica el origen de los datos para el procesamiento.
        /// Puede ser un archivo(File) o una base de datos(Database).
        /// </summary>
        [DefaultValue(DataSourceType.File)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DataSourceType? SourceType { get; set; } = DataSourceType.File;
        public string? Province { get; set; } = null;
        public string? Municipality { get; set; } = null;
        public string? Sector { get; set; } = null;
        public string? Level { get; set; } = null!;
        public string? TypeCenter { get; set; } = null!;
        public string? Area { get; set; } = null!;
        public bool? isOffices { get; set; } = null!;
        public bool? isDentistry { get; set; } = null!;
        public bool? isEmergency { get; set; } = null!;
        public bool? isLaboratory { get; set; } = null!;
        public bool? isSonography { get; set; } = null!;
        public bool? isPhysiotherapy { get; set; } = null!;
        public bool? isInternet { get; set; } = null!;
        public bool? Xray { get; set; } = null!;

    }

    /// <summary>
    /// Tipos de origen de datos.
    /// </summary>
    public enum DataSourceType
    {
        /// <summary>
        /// "Archivo local o en la nube"
        /// </summary>
        File,

        /// <summary>
        /// Base de datos relacional
        /// </summary>
        Database
    }
}
