using Hangfire;
using HealthCenterAPI.Application.Services;
using HealthCenterAPI.Domain.Contracts.IRepository;
using HealthCenterAPI.Domain.Contracts.IServices;
using HealthCenterAPI.Extencion;
using HealthCenterAPI.Infraestructura.Repository;
using HealthCenterAPI.Presentation.Mapping;
using HealthCenterAPI.Shared.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json.Serialization;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);




builder.Services.ConfigureDbPostgreSQL(builder.Configuration);
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureHangFire(builder.Configuration);
builder.Services.ConfigureBackgroundJobs();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient<WebScrapingRIESS>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFileServices, FileServices>();
builder.Services.AddScoped<IHealthCenterServices, HealthCenterServices>();
builder.Services.AddScoped<IHealthCenterRepository, HealthCenterRepository>();

builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
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
    app.MapOpenApi();
    app.UseHangfireDashboard();
}

// Configurar Scalar para la interfaz de usuario
app.MapScalarApiReference(options =>
{
    options.WithTitle("HealthCenterAPI")
           .WithDownloadButton(true)
           .WithTheme(ScalarTheme.Purple)
           .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios);
});
app.UseCors("AllowAll");
app.UseRouting();
app.MapControllers();
app.Run();


