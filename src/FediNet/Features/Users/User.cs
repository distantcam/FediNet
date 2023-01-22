using FediNet.ActivityStreams;
using FediNet.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FediNet.Features.Users;

public class User : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .MapGet("/users/{username}", Handler)
        .Produces<Actor>(200,
        "application/ld+json; profile=\"https://www.w3.org/ns/activitystreams\"",
        "application/activity+json");

    private static JsonHttpResult<Actor> Handler(string username, UriGenerator _uriGenerator)
    {
        var activity = new Actor
        {
            Context = "https://www.w3.org/ns/activitystreams",
            Id = _uriGenerator.GetCurrentUri(),
            Type = "Person",
            //Inbox = _uriGenerator.GetUriByName(nameof(Inbox), new { username })!,
            //Outbox = _uriGenerator.GetUriByName(nameof(Outbox), new { username })!,
            Name = username,
            //PreferredUsername = username, // Needed to support mastodon
            Url = _uriGenerator.GetUriByName(nameof(Profile), new { username }) // Mastodon uses this for profile link
        };

        return TypedResults.Json(activity, contentType: "application/ld+json; profile=\"https://www.w3.org/ns/activitystreams\"");
    }
}
