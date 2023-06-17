using FediNet.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FediNet.Features.WellKnown;

public class NodeInfo : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .MapGet("/.well-known/nodeinfo", Handler);

    public record Response(IEnumerable<Link> Links);

    private static Ok<Response> Handler(UriGenerator uriGenerator)
    {
        var response = new Response(new[]
        {
            Link.Create("http://nodeinfo.diaspora.software/ns/schema/2.0",
            uriGenerator.GetUriByName(nameof(NodeInfoV20)))
        });
        return TypedResults.Ok(response);
    }
}
