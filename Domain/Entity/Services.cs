using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthCenterAPI.Domain.Entity
{
    public class Services
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid? HealthCenterId { get; set; }

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
        public bool? isInternet { get; set; } = null!;

        [AllowNull]
        public bool? Xray { get; set; } = null!;

        public  virtual HealthCenter? HealthCenter { get; set; } = null!;

    }
}
