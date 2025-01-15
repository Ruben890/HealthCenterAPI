using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthCenterAPI.Domain.Entity
{
    public class HealthCenter
    {
        [Key]
        public Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.UtcNow);

        [Required]
        [Unicode(true)]
        public string? Name { get; set; }

        [Required]
        public string? Level { get; set; }

        [Required]
        public string? TypeCenter { get; set; }

        [AllowNull]
        public string? SRS { get; set; } = null!;

        [AllowNull]
        public string? Tel { get; set; } = null!;

        [AllowNull]
        public string? RNC { get; set; } = null!;

        [AllowNull]
        public string Email { get; set; } = null!;

        [AllowNull]
        public string Fax { get; set; } = null!;

        [Required]
        public int? OpeningYear {  get; set; }

        [AllowNull]
        public int? lastRenovationYear { get; set; } = null!;

        [AllowNull]
        public string? Managed_By { get; set; } = null!;

        [AllowNull]
        public string? ServiceComplexity { get; set; } = null!;

        public virtual Location? Location { get; set; } = null!;

        public virtual Services Services { get; set; } = null!;
    }
}
