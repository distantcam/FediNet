using AutoCtor;
using EndpointConfigurator;
using FediNet.Extensions;
using FediNet.Infrastructure;
using FediNet.Services;

namespace FediNet.Features.WellKnown;

public static partial class WebFinger
{
    public record Request(string Resource) : IHttpRequest;

    public record Response(string Subject, string[]? Aliases, Link[]? Links);

    public record Link(string Rel, string? Type, string? Href, string? Template);

    [AutoConstruct]
    public partial class Handler : SyncHttpRequestHandler<Request>
    {
        [EndpointConfig]
        public static void Config(IEndpointRouteBuilder app) =>
            app.MediateGet<Request>("/.well-known/webfinger")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

        private readonly UriGenerator _uriGenerator;

        public override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            if (!AcctUri.TryParse(request.Resource, out var acct))
                return Results.BadRequest();

            var userPage = _uriGenerator.GetUriByName("UserPage", new { username = acct.User })!;

            var response = new Response(
                acct.ToString(),
                new[] { userPage },
                new[] {
                    new Link("self", "application/activity+json", userPage, null)
                });

            return Results.Ok(response);
        }
    }
}
