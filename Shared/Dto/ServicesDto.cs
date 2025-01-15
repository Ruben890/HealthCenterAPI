using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthCenterAPI.Shared.Dto
{
    public class ServicesDto
    {
      
        [AllowNull]
        public bool? isOffices { get; set; } = null!;
        [AllowNull]
        public bool? isDentistry { get; set; } = null!;
        [AllowNull]
        public bool? isEmergency { get; set; } = null!;
        [AllowNull]
        public bool? isLaboratory { get; set; } = null!;
        [AllowNull]
        public bool? isSonography { get; set; } = null!;
        [AllowNull]
        public bool? isPhysiotherapy { get; set; } = null!;
        [AllowNull]
        public bool? isComputer { get; set; } = null!;
        [AllowNull]
        public bool? isInternet { get; set; } = null!;
        [AllowNull]
        public bool? isRayoxX { get; set; } = null!;
    }
}
