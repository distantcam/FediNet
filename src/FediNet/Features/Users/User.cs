using FediNet.Services;
using KristofferStrube.ActivityStreams;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FediNet.Features.Users;

public class User : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .MapGet("/users/{username}", Handler)
        .Produces<Person>(200,
        "application/ld+json; profile=\"https://www.w3.org/ns/activitystreams\"",
        "application/activity+json");

    private static JsonHttpResult<Person> Handler(string username, UriGenerator _uriGenerator)
    {
        var person = new Person
        {
            Id = _uriGenerator.GetCurrentUri(),
            Name = new[] { username },
            PreferredUsername = username, // Needed to support mastodon
            Inbox = _uriGenerator.GetLinkByName(nameof(Inbox), new { username }),
            Outbox = _uriGenerator.GetLinkByName(nameof(Outbox), new { username }),
            Url = new[] { _uriGenerator.GetLinkByName(nameof(Profile), new { username }) } // Mastodon uses this for profile link
        };

        return TypedResults.Json(person, contentType: "application/ld+json; profile=\"https://www.w3.org/ns/activitystreams\"");
    }
}
