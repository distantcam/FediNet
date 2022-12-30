using System.Text.Json.Serialization;
using FediNet;
using FediNet.Infrastructure;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Serilog;

[assembly: MediatorOptions(ServiceLifetime = ServiceLifetime.Scoped)]

#pragma warning disable RS0030 // Do not used banned APIs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((context, services, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();
    });

    var connectionString = builder.Configuration.GetConnectionString("FediNetDb");
    if (string.IsNullOrEmpty(connectionString))
        throw new Exception("Database connection string not set.");
    if (builder.Environment.IsDevelopment())
    {
        var dbUpdater = new DatabaseUpgrader(connectionString);
        dbUpdater.PerformUpgrade(create: true);
    }
    builder.Services
        .AddDbContext<FediNetContext>(options => options
            .UseSqlServer(connectionString, opts => opts
                .MigrationsAssembly(typeof(FediNetContext).Assembly.FullName)),
        optionsLifetime: ServiceLifetime.Singleton) // For scoped access use DbContext
        .AddDbContextFactory<FediNetContext>(lifetime: ServiceLifetime.Singleton); // For singleton access use DbContextFactory

    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

    builder.Services.AddMediator();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.CustomSchemaIds(t => t.FullName?.Replace('+', '.'));
    });

    // Set up pipeline
    builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

    builder.Services.Scan(scan => scan.FromAssemblyOf<Program>()
        // Register services
        .AddClasses(classes => classes.InNamespaces("FediNet.Services"))
            .AsImplementedInterfaces()
            .AsSelf()
            .WithScopedLifetime()
    );

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseHttpsRedirection();
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
