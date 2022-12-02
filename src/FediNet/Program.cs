using FediNet.Extensions;
using FediNet.Features.WellKnown;
using FediNet.Infrastructure;
using FluentValidation;
using Mediator;
using Serilog;

#pragma warning disable RS0030 // Do not used banned APIs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddSerilog();

    builder.Services.AddMediator();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();

    app.MapGetRequest<NodeInfo.Request>("/.well-known/nodeinfo");

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
