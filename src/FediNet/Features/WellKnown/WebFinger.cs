using AutoCtor;
using Dunet;
using FediNet.Features.Users;
using FediNet.Services;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FediNet.Features.WellKnown;

public partial class WebFinger : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .SendGet<Request, Response, Results<Ok<Response.UserDetails>, NotFound>>(
            "/.well-known/webfinger",
            (string resource) => new Request(resource),
            response => response.Match<Results<Ok<Response.UserDetails>, NotFound>>(
                r => TypedResults.Ok(r),
                nf => TypedResults.NotFound()));

    public record Request(string Resource) : IRequest<Response>;
    [Union]
    public partial record Response
    {
        public partial record UserDetails(string Subject, string[]? Aliases, Link[]? Links);
        public partial record NotFoundResponse;
    }

    [AutoConstruct]
    public partial class Handler : SyncHandler<Request, Response>
    {
        private readonly UriGenerator _uriGenerator;

        protected override Response Handle(Request request)
        {
            if (!AcctUri.TryParse(request.Resource, out var acct))
                return new Response.NotFoundResponse();

            var userPage = _uriGenerator.GetUriByName(nameof(User), new { username = acct.User })!;

            var userDetails = new Response.UserDetails(
                acct.ToString(),
                new[] { userPage },
                new[] {
                            Link.Create("self", "application/activity+json", userPage)
                });

            return userDetails;
        }
    }
}
