using Hangfire;
using HealthCenterAPI.Contracts.IRepository;
using HealthCenterAPI.Contracts.IServices;
using HealthCenterAPI.Domain.Services;
using HealthCenterAPI.Extencion;
using HealthCenterAPI.Infraestructura.Repository;
using HealthCenterAPI.Shared.Utils;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);




builder.Services.ConfigureDbPostgreSQL(builder.Configuration);
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureHangFire(builder.Configuration);
builder.Services.ConfigureBackgroundJobs();
builder.Services.AddTransient<WebScrapingRIESS>();
builder.Services.AddTransient<IFileRepository, FileRepository>();
builder.Services.AddTransient<IFileServices, FileServices>();
builder.Services.ConfigurationCords();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseHsts();    // Habilitar HSTS (HTTP Strict Transport Security)

    // A�adir encabezados de seguridad
    app.Use(async (context, next) =>
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["Referrer-Policy"] = "no-referrer";
        context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
        context.Response.Headers["Content-Security-Policy"] = "default-src 'none'; frame-ancestors 'none'; script-src 'none';"; // Restrict APIs
        await next();
    });

    app.UseHttpsRedirection();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
});

// Activar el dashboard de Hangfire solo en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard();
}

app.UseCors("AllowAll");
app.UseRouting();
app.MapControllers();
app.Run();


