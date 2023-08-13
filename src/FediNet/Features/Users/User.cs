using AutoCtor;
using FediNet.Services;
using KristofferStrube.ActivityStreams;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FediNet.Features.Users;

public partial class User : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .SendGet<Request, Person, Ok<Person>>(
            "/users/{username}",
            (string username) => new Request(username),
            response => TypedResults.Ok(response))
        .Produces<Person>(200,
            "application/ld+json; profile=\"https://www.w3.org/ns/activitystreams\"",
            "application/activity+json");

    public record Request(string Username) : IRequest<Person>;

    [AutoConstruct]
    public partial class Handler : SyncHandler<Request, Person>
    {
        private readonly UriGenerator _uriGenerator;

        protected override Person Handle(Request request)
        {
            var username = request.Username;
            var person = new Person
            {
                Id = _uriGenerator.GetCurrentUri(),
                Name = new[] { username },
                PreferredUsername = username, // Needed to support mastodon
                Inbox = _uriGenerator.GetLinkByName(nameof(Inbox), new { username }),
                Outbox = _uriGenerator.GetLinkByName(nameof(Outbox), new { username }),
                Url = new[] { _uriGenerator.GetLinkByName(nameof(Profile), new { username }) } // Mastodon uses this for profile link
            };
            return person;
        }
    }
}
