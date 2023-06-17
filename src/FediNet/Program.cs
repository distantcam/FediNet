using System.Text.Json.Serialization;
using Serilog;

#pragma warning disable RS0030 // Do not used banned APIs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Logging
    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((context, services, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();
    });

    // OpenApi
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        // Fixes schema ids for nested types
        c.CustomSchemaIds(t => t.FullName?.Replace('+', '.'));
    });

    // JsonSerializer
    builder.Services.ConfigureHttpJsonOptions(o =>
    {
        o.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

    // Services
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddHttpClient();

    builder.Services.Scan(scan => scan.FromAssemblyOf<Program>()
        // Register services
        .AddClasses(classes => classes.InNamespaces("FediNet.Services"))
            .AsImplementedInterfaces()
            .AsSelf()
            .WithScopedLifetime()
        // Register caches
        .AddClasses(classes => classes.InNamespaces("FediNet.Caching"))
            .AsSelf()
            .WithSingletonLifetime()
    );

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();

    app.MapEndpoints();

    app.Run();
}
catch (HostAbortedException) { /* EF Migrations */ }
finally
{
    Log.CloseAndFlush();
}
