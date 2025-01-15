using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HealthCenterAPI.Repository
{
    public class HealthCenterContex :DbContext
    {
        public HealthCenterContex(DbContextOptions<HealthCenterContex> options):base(options) { }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
