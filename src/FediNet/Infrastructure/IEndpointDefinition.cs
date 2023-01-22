using System.Reflection;

public interface IEndpointDefinition
{
    static abstract void MapEndpoint(IEndpointRouteBuilder builder);
}

public interface IEndpointGroupDefinition
{
    static abstract void MapEndpoint(RouteGroupBuilder builder);
}

public static class EndpointRouteBuilderExtensions
{
    private static readonly MethodInfo MapEndpointMethod = typeof(EndpointRouteBuilderExtensions)
        .GetMethod(nameof(MapEndpoint), BindingFlags.NonPublic | BindingFlags.Static)!;
    private static readonly MethodInfo MapGroupEndpointMethod = typeof(EndpointRouteBuilderExtensions)
        .GetMethod(nameof(MapGroupEndpoint), BindingFlags.NonPublic | BindingFlags.Static)!;

    private static void MapEndpoint<T>(IEndpointRouteBuilder builder) where T : IEndpointDefinition => T.MapEndpoint(builder);
    private static void MapGroupEndpoint<T>(RouteGroupBuilder builder) where T : IEndpointGroupDefinition =>
        T.MapEndpoint(builder);

    public static void MapEndpoints(this IEndpointRouteBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var endpointDefinitions = Assembly.GetCallingAssembly().GetTypes()
            .Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t) && !t.IsInterface);
        foreach (var endpointDefinition in endpointDefinitions)
        {
            MapEndpointMethod.MakeGenericMethod(endpointDefinition).Invoke(null, new[] { builder });
        }

        var groupEndpointDefinitions = Assembly.GetCallingAssembly().GetTypes()
            .Where(t => typeof(IEndpointGroupDefinition).IsAssignableFrom(t) && !t.IsInterface);
        foreach (var groupEndpointDefinition in groupEndpointDefinitions)
        {
            var tag = groupEndpointDefinition.Namespace?.Split('.').Last();
            var groupBuilder = builder.MapGroup("").WithName(groupEndpointDefinition.Name);
            if (!string.IsNullOrEmpty(tag))
                groupBuilder = groupBuilder.WithTags(tag);
            MapGroupEndpointMethod.MakeGenericMethod(groupEndpointDefinition).Invoke(null, new[] { groupBuilder });
        }
    }
}
