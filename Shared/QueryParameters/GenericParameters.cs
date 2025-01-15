using HealthCenterAPI.Shared.RequestFeatures;
using System.Diagnostics.CodeAnalysis;

namespace HealthCenterAPI.Shared.QueryParameters
{
    public class GenericParameters : RequestParameters
    {
        public string? Province { get; set; } = null;
        public string? Municipality { get; set; } = null;
        public string? Sector {  get; set; } = null;
        public string? Level { get; set; } = null!;
        public string? TypeCenter { get; set; } = null!;
        public string Area { get; set; } = null!;
        public bool? isOffices { get; set; } = null!;
        public bool? isDentistry { get; set; } = null!;
        public bool? isEmergency { get; set; } = null!;
        public bool? isLaboratory { get; set; } = null!;
        public bool? isSonography { get; set; } = null!;
        public bool? isPhysiotherapy { get; set; } = null!;
        public bool? isInternet { get; set; } = null!;
        public bool? Xray { get; set; } = null!;

    }
}
