using FediNet.ActivityStreams;
using FediNet.Infrastructure;
using FediNet.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FediNet.Features.Users;

public class User : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .MapGet("/users/{username}", Handler);

    private static Results<JsonHttpResult<Actor>, BadRequest> Handler(/*[FromHeader(Name = "accept")] string? accept,*/ string username, UriGenerator _uriGenerator)
    {
        //var accepted = new[] {
        //        "application/ld+json; profile=\"https://www.w3.org/ns/activitystreams\"",
        //    "application/activity+json"
        //};

        //if (string.IsNullOrEmpty(accept) ||
        //    !accept.Split(',').Select(a => a.Trim()).Intersect(accepted).Any())
        //    return TypedResults.BadRequest();

        var activity = new Actor
        {
            Context = "https://www.w3.org/ns/activitystreams",
            Id = _uriGenerator.GetCurrentUri(),
            Type = "Person",
            Inbox = _uriGenerator.GetUriByName(nameof(Inbox), new { username })!,
            Outbox = _uriGenerator.GetUriByName(nameof(Outbox), new { username })!,
            Name = username,
            PreferredUsername = username, // Needed to support mastodon
            Url = _uriGenerator.GetUriByName(nameof(Profile), new { username }) // Mastodon uses this for profile link
        };

        return TypedResults.Json(activity, contentType: "application/ld+json; profile=\"https://www.w3.org/ns/activitystreams\"");
    }
}
