using EndpointConfigurator;

namespace FediNet.Features.Users;

public static partial class Outbox
{
    [EndpointConfig]
    public static void Config(IEndpointRouteBuilder app) =>
        app.MapGet("/users/{username}/outbox", () => Results.StatusCode(501))
            .WithName(nameof(Outbox));
}
