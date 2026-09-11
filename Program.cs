using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using Serilog;
using SuperVet.Services;
using SuperVet.Data;
using System.IO;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

// Configura o Serilog no host para que UseSerilogRequestLogging funcione corretamente
// Garante que a pasta de logs exista
Directory.CreateDirectory(Path.Combine(builder.Environment.ContentRootPath, "logs"));

builder.Host.UseSerilog((ctx, services, config) =>
{
    config
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(Path.Combine(builder.Environment.ContentRootPath, "logs", "supervet.txt"),
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 7);
});
// OpenTelemetry — Tracing e Métricas
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter();
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter();
    });

// Registra o DbContext de produção apenas fora do ambiente de testes de integração
if (!builder.Environment.IsEnvironment("IntegrationTests"))
{
    var connectionString = builder.Configuration.GetConnectionString("OracleConnection");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(connectionString, b => b.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19)));
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Register application services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddHealthChecks()
    .AddCheck("api", () => HealthCheckResult.Healthy("API está funcionando."))
    .AddDbContextCheck<AppDbContext>("oracle-db");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "SuperVet API";
    });
}
app.MapHealthChecks("/health/detail", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.UseSerilogRequestLogging();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();