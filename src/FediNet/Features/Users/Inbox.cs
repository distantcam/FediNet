using FediNet.Infrastructure;

namespace FediNet.Features.Users;

public partial class Inbox : IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .MapPost("/users/{username}/inbox", () => Results.StatusCode(501))
        .RequireAuthorization()
        .WithName(nameof(Inbox));
}
