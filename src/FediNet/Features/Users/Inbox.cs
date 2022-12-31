using EndpointConfigurator;

namespace FediNet.Features.Users;

public static partial class Inbox
{
    [EndpointConfig]
    public static void Config(IEndpointRouteBuilder app) =>
        app.MapPost("/users/{username}/inbox", () => Results.StatusCode(501))
            .RequireAuthorization()
            .WithName(nameof(Inbox));
}
