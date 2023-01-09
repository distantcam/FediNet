using System.Reflection;

namespace FediNet.Infrastructure;

public interface IEndpointDefinition
{
    static abstract void MapEndpoint(IEndpointRouteBuilder builder);
}

public static class EndpointRouteBuilderExtensions
{
    private static readonly MethodInfo MapEndpointMethod = typeof(EndpointRouteBuilderExtensions).GetMethod(nameof(MapEndpoint), BindingFlags.NonPublic | BindingFlags.Static)!;
    private static void MapEndpoint<T>(IEndpointRouteBuilder builder) where T : IEndpointDefinition => T.MapEndpoint(builder);

    public static void MapEndpoints(this IEndpointRouteBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var endpointDefinitions = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t) && !t.IsInterface);
        foreach (var endpointDefinition in endpointDefinitions)
        {
            MapEndpointMethod.MakeGenericMethod(endpointDefinition).Invoke(null, new[] { builder });
        }
    }
}
