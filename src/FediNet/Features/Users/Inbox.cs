using FediNet.Infrastructure;

namespace FediNet.Features.Users;

public class Inbox : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .MapPost("/users/{username}/inbox", () => Results.StatusCode(501));
}
