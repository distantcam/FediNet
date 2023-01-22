using FediNet.Infrastructure;

namespace FediNet.Features.Users;

public class Profile : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .MapGet("/@{username}", () => Results.StatusCode(501));
}
