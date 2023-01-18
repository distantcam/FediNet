using AutoCtor;
using FediNet.Extensions;
using FediNet.Features.Users;
using FediNet.Infrastructure;
using FediNet.Models;
using FediNet.Services;

namespace FediNet.Features.WellKnown;

public partial class WebFinger : Feature, IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .MediateGet<Request>("/.well-known/webfinger")
        .Produces<Response>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName(nameof(WebFinger));

    public record Request(string Resource) : IFeatureRequest;

    public record Response(string Subject, string[]? Aliases, Link[]? Links);

    [AutoConstruct]
    public partial class Handler : SyncFeatureHandler<Request>
    {
        private readonly UriGenerator _uriGenerator;

        protected override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            if (!AcctUri.TryParse(request.Resource, out var acct))
                return TypedResults.NotFound();

            var userPage = _uriGenerator.GetUriByName(nameof(User), new { username = acct.User })!;

            var response = new Response(
                acct.ToString(),
                new[] { userPage },
                new[] {
                    Link.Create("self", "application/activity+json", userPage)
                });

            return TypedResults.Ok(response);
        }
    }
}
