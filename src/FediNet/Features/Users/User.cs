using AutoCtor;
using FediNet.Extensions;
using FediNet.Infrastructure;
using FediNet.Models.ActivityStreams;
using FediNet.Services;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace FediNet.Features.Users;

public partial class User : IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .MediateGet<Request>("/users/{username}")
        .Produces(StatusCodes.Status400BadRequest)
        .Produces<Actor>(StatusCodes.Status200OK)
        .WithName(nameof(User));

    public record Request([FromHeader] string? Accept, string Username) : IRequest<IResult>;

    [AutoConstruct]
    public partial class Handler : SyncRequestHandler<Request, IResult>
    {
        private readonly UriGenerator _uriGenerator;

        protected override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Accept) || !request.Accept.Split(',')
                .Any(a => a.Trim().Equals("application/ld+json", StringComparison.InvariantCultureIgnoreCase)))
                return Results.BadRequest();

            var activity = new Actor
            {
                Context = Constants.ActivityStreamsContext,
                Id = _uriGenerator.GetCurrentUri(),
                Type = "Person",
                Inbox = _uriGenerator.GetUriByName(nameof(Inbox), new { username = request.Username })!,
                Outbox = _uriGenerator.GetUriByName(nameof(Outbox), new { username = request.Username })!,
                Name = request.Username,
                PreferredUsername = request.Username, // Needed to support mastodon
                Url = _uriGenerator.GetUriByName(nameof(Profile), new { username = request.Username }) // Mastodon uses this for profile link
            };

            return Results.Extensions.JsonActivity(activity);
        }
    }
}
