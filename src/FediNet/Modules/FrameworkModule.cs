using Autofac;
using FediNet.Infrastructure;
using MediatR;

namespace FediNet.Modules;

public class FrameworkModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterMediatorRequestHandlers(ThisAssembly);

        // Pipeline order
        builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterGeneric(typeof(ValidationBehaviour<,>)).AsImplementedInterfaces().InstancePerLifetimeScope();
    }
}
