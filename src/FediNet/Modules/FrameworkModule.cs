using Autofac;
using FediNet.Infrastructure;
using Mediator;

namespace FediNet.Modules;

public class FrameworkModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Register handlers
        builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IRequestHandler<,>));

        // Pipeline order
        builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterGeneric(typeof(ValidationBehaviour<,>)).AsImplementedInterfaces().InstancePerLifetimeScope();
    }
}
