using System.ComponentModel.DataAnnotations;

namespace HealthCenterAPI.Domain.Entity
{
    public class Versioning
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Version { get; set; }
        [Required]
        public int PreviousVersion { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
