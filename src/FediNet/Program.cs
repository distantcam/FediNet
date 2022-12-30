using System.Text.Json.Serialization;
using FediNet.Infrastructure;
using FluentValidation;
using Mediator;
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
finally
{
    Log.CloseAndFlush();
}
