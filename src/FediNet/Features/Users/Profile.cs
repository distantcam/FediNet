using EndpointConfigurator;

namespace FediNet.Features.Users;

public static partial class Profile
{
    [EndpointConfig]
    public static void Config(IEndpointRouteBuilder app) =>
        app.Map("/@{username}", (string username) => Results.StatusCode(501))
            .WithName(nameof(Profile));
}
