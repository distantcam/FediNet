using EndpointConfigurator;

namespace FediNet.Features.Development;

public static class Dummy
{
    [EndpointConfig]
    public static void Config(IEndpointRouteBuilder app)
    {
        app.MapGet("/@{username}", (string username) => Results.NotFound())
            .WithName("ProfilePage");
        app.MapGet("/users/{username}", (string username) => Results.NotFound())
            .WithName("UserPage");
        app.MapGet("/authorize_interaction", (string uri) => Results.NotFound())
            .WithName("subscribe");
    }
}
