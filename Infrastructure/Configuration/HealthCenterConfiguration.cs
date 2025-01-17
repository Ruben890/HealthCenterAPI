using HealthCenterAPI.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthCenterAPI.Infrastructure.Configuration
{
    public class HealthCenterConfiguration : IEntityTypeConfiguration<HealthCenter>
    {
        public void Configure(EntityTypeBuilder<HealthCenter> entity)
        {
            entity.ToTable(nameof(HealthCenter));

            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Name);


            entity.HasOne(hc => hc.Services)
                    .WithOne(s => s.HealthCenter)
                    .HasForeignKey<Services>(s => s.HealthCenterId)
                    .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(hc => hc.Location)
              .WithOne(l => l.HealthCenter)
              .HasForeignKey<Location>(l => l.HealthCenterId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
