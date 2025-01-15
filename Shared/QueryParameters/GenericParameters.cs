using HealthCenterAPI.Shared.RequestFeatures;

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
    }
}
