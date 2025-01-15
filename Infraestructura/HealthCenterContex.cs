using HealthCenterAPI.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HealthCenterAPI.Repository
{
    public class HealthCenterContex :DbContext
    {
        public HealthCenterContex(DbContextOptions<HealthCenterContex> options):base(options) { }


        public virtual DbSet<HealthCenter> HealthCenters { get; set; } = null!;

        public virtual DbSet<Location> Locations { get; set; } = null!;

        public virtual DbSet<Services> Services { get; set; } = null!;  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
