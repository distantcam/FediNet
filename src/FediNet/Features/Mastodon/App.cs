using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FediNet.Features.Mastodon;

public class App : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .MapPost("/api/v1/apps", Handler);

    public record Request(
        [property: JsonPropertyName("client_name")] string ClientName,
        [property: JsonPropertyName("redirect_uris")] string RedirectUris,
        string? Scopes,
        string? Website
    );
    public record Response;

    private static StatusCodeHttpResult Handler([FromBody] Request request)
    {
        return TypedResults.StatusCode(501);
    }
}
