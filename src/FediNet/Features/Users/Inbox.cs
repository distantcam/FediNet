using EndpointConfigurator;

namespace FediNet.Features.Users;

public static partial class Inbox
{
    [EndpointConfig]
    public static void Config(IEndpointRouteBuilder app) =>
        app.MapGet("/users/{username}/inbox", () => Results.StatusCode(501))
            .WithName(nameof(Inbox));
}
