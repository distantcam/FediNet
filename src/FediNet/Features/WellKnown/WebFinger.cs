using AutoCtor;
using FediNet.Extensions;
using FediNet.Features.Users;
using FediNet.Infrastructure;
using FediNet.Models;
using FediNet.Services;
using Mediator;

namespace FediNet.Features.WellKnown;

public partial class WebFinger : IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .MediateGet<Request>("/.well-known/webfinger")
        .Produces<Response>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName(nameof(WebFinger));

    public record Request(string Resource) : IRequest<IResult>;

    public record Response(string Subject, string[]? Aliases, Link[]? Links);

    [AutoConstruct]
    public partial class Handler : SyncRequestHandler<Request, IResult>
    {
        private readonly UriGenerator _uriGenerator;

        protected override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            if (!AcctUri.TryParse(request.Resource, out var acct))
                return Results.NotFound();

            var userPage = _uriGenerator.GetUriByName(nameof(User), new { username = acct.User })!;

            var response = new Response(
                acct.ToString(),
                new[] { userPage },
                new[] {
                    Link.Create("self", "application/activity+json", userPage)
                });

            return Results.Ok(response);
        }
    }
}
