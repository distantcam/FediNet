using System.Web;
using AutoCtor;
using EndpointConfigurator;
using FediNet.Extensions;
using FediNet.Infrastructure;

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
            app.MediateGet<Request>("/.well-known/webfinger");

        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            var acct = new AcctUri(request.Resource);

            var httpContext = _httpContextAccessor.HttpContext!;

            var profilePage = _linkGenerator.GetUriByName(httpContext, "ProfilePage", new { username = acct.User })!;
            var userPage = _linkGenerator.GetUriByName(httpContext, "UserPage", new { username = acct.User })!;
            var subscribeLink = HttpUtility.UrlDecode(_linkGenerator.GetUriByName(httpContext, "subscribe", new { uri = "{{uri}}" }));

            var response = new Response(
                acct.ToString(),
                new[] { profilePage, userPage },
                new[] {
                    new Link("http://webfinger.net/rel/profile-page", "text/html", profilePage, null),
                    new Link("self", "application/activity+json", userPage, null),
                    new Link("http://ostatus.org/schema/1.0/subscribe", null, null, subscribeLink)
                });

            return Results.Ok(response);
        }
    }
}
