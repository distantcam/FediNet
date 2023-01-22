using FediNet.Features.Users;
using FediNet.Services;

namespace FediNet.Features.WellKnown;

public class WebFinger : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .MapGet("/.well-known/webfinger", Handler);

    public record Response(string Subject, string[]? Aliases, Link[]? Links);

    private static IResult Handler(UriGenerator uriGenerator, string resource)
    {
        if (!AcctUri.TryParse(resource, out var acct))
            return TypedResults.NotFound();

        var userPage = uriGenerator.GetUriByName(nameof(User), new { username = acct.User })!;

        var response = new Response(
            acct.ToString(),
            new[] { userPage },
            new[] {
                    Link.Create("self", "application/activity+json", userPage)
            });

        return TypedResults.Ok(response);
    }
}
