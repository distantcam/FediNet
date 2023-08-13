using AutoCtor;
using FediNet.Services;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FediNet.Features.WellKnown;

public partial class NodeInfo : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .SendGet<Request, Response, Ok<Response>>(
            "/.well-known/nodeinfo",
            () => new Request(),
            response => TypedResults.Ok(response));

    public record Request : IRequest<Response>;
    public record Response(IEnumerable<Link> Links);

    [AutoConstruct]
    public partial class Handler : SyncHandler<Request, Response>
    {
        private readonly UriGenerator _uriGenerator;

        protected override Response Handle(Request request)
        {
            var response = new Response(new[]
            {
                Link.Create("http://nodeinfo.diaspora.software/ns/schema/2.0",
                _uriGenerator.GetUriByName(nameof(NodeInfoV20)))
            });
            return response;
        }
    }
}
