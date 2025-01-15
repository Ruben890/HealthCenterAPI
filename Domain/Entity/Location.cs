using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthCenterAPI.Domain.Entity
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid? HealthCenterId { get; set; }

        [Required]
        public string? Province { get; set; }

        [Required]
        public string? Municipality { get; set; }

        [Required]
        public string? MunicipalDistrict { get; set; }

        [Required]
        public string? Sector {  get; set; }

        [AllowNull]
        public string? Address { get; set; } = null!;

        [Required]
        public string? Neighborhood { get; set; }

        [Required]
        public string? Area { get; set; }

        [Required]
        public string? Zone { get; set; }

        [AllowNull]
        public Point? Ubication { get; set; } = null!;

        public virtual HealthCenter? HealthCenter { get; set; } = null!;
    }
}
