using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace MediatR;

public static class MediatorContainerBuilderExtensions
{
    public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterMediatorRequestHandlers(
        this ContainerBuilder builder, params Assembly[] assemblies)
    {
        return builder.RegisterAssemblyTypes(assemblies)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));
    }
}
