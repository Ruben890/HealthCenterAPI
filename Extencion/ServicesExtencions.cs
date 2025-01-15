using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.PostgreSql;
using HealthCenterAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace HealthCenterAPI.Extencion
{
    public static class ServicesExtencions
    {
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options => { });

        public static void ConfigureDbPostgreSQL(this IServiceCollection services, IConfiguration config)
        {
            // Configuración de PostgreSQL para la base de datos
            services.AddDbContext<HealthCenterContex>(options =>
                options.UseNpgsql(
                    config.GetConnectionString("DbConection"),
                    npgsqlOptions => npgsqlOptions.UseNetTopologySuite()
                )
            );
        }

        public static void ConfigurationCords(this IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                    .WithMethods("GET")
                    .WithHeaders();
                });

            });
        }

        public static void ConfigureHangFire(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("GoTravelConnection");

            services.AddHangfire(config =>
            {
                if (configuration.GetValue<bool>("DataSourceType:Database", false))
                {
                    config.UsePostgreSqlStorage(c =>
                    c.UseNpgsqlConnection(connectionString), new PostgreSqlStorageOptions
                    {
                        InvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.FromSeconds(15),
                        DistributedLockTimeout = TimeSpan.FromMinutes(10),
                        UseNativeDatabaseTransactions = true

                    });
                }
                else config.UseMemoryStorage();
            });

            services.AddHangfireServer();
        }
    }
}
