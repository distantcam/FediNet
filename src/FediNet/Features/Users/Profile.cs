using FediNet.Infrastructure;

namespace FediNet.Features.Users;

public partial class Profile : IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .Map("/@{username}", (string username) => Results.StatusCode(501))
        .WithName(nameof(Profile));
}
