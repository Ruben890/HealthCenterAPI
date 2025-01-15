using HealthCenterAPI.Repository;
using HealthCenterAPI.Shared.Utils;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddTransient<WebScrapingRIESS>();

builder.Services.AddNpgsql<HealthCenterContex>(
    builder.Configuration.GetConnectionString("DbConection"),
    options => options.UseNetTopologySuite());


builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
        .WithMethods("GET")
        .WithHeaders();
    });

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

app.UseCors("AllowAll");
app.UseRouting();
app.MapControllers();
app.Run();


