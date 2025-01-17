using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.PostgreSql;
using HealthCenterAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

        public static void ConfigurationCords(this IServiceCollection services,IConfiguration config)
        {

            // Leer la lista de orígenes permitidos desde la configuración
            var allowedOrigins = config.GetSection("AllowedOrigins").Get<string[]>() ?? ["*"];

            if (allowedOrigins is null || allowedOrigins.Length == 0)
            {
                throw new InvalidOperationException("No allowed origins have been defined in the settings. Make sure to add the 'AllowedOrigins' section in appsettings.");
            }

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins(allowedOrigins)
                           .AllowAnyHeader() // Permitir todos los encabezados
                           .WithMethods("GET")
                           .AllowCredentials() // Habilitar credenciales
                           .WithExposedHeaders("X-Custom-Header") // Exponer solo los encabezados necesarios
                           .SetPreflightMaxAge(TimeSpan.FromMinutes(10)); // Cacheo de preflight (OPTIONS)

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
