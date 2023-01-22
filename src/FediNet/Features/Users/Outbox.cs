using FediNet.Infrastructure;

namespace FediNet.Features.Users;

public class Outbox : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .MapGet("/users/{username}/outbox", () => Results.StatusCode(501));
}
