using FluentValidation;
using MediatR;

namespace FediNet;

// Startup configures services that are not used in tests.
// For any services needed in tests, use an Autofac module instead.
public static class Startup
{
    public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration, IHostEnvironment hostEnvironment)
    {
        var coreAssembly = typeof(Startup).Assembly;

        services.AddValidatorsFromAssembly(coreAssembly);
        services.AddMediatR(coreAssembly);
    }
}
