using System.Text.Json.Serialization;
using FediNet.Extensions;
using FediNet.Features.WellKnown;
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
            .WithScopedLifetime()
    );

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();

    app.MediateGet<NodeInfo.Request>("/.well-known/nodeinfo");
    app.MediateGet<NodeInfoV20.Request>("/nodeinfo/2.0.json")
        .WithName("NodeInfoV2");
    app.MediateGet<WebFinger.Request>("/.well-known/webfinger");

    app.MapGet("/@{username}", (string username) => Results.NotFound())
        .WithName("ProfilePage");
    app.MapGet("/users/{username}", (string username) => Results.NotFound())
        .WithName("UserPage");
    app.MapGet("/authorize_interaction", (string uri) => Results.NotFound())
        .WithName("subscribe");

    app.Run();
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        // see https://github.com/dotnet/runtime/issues/60600
        throw;
    }

    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}
