using AutoCtor;
using FediNet.Extensions;
using FediNet.Infrastructure;
using FediNet.Services;

namespace FediNet.Features.WellKnown;

public partial class NodeInfo : Feature, IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .MediateGet<Request>("/.well-known/nodeinfo")
        .Produces<Response>(StatusCodes.Status200OK);

    public record Request : IFeatureRequest;

    public record Response(IEnumerable<Link> Links);

    public record Link(string Href, string Rel);

    [AutoConstruct]
    public partial class Handler : SyncFeatureHandler<Request>
    {
        private readonly UriGenerator _uriGenerator;

        protected override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            var response = new Response(new[]
            {
                new Link("http://nodeinfo.diaspora.software/ns/schema/2.0",
                _uriGenerator.GetUriByName(nameof(NodeInfoV20)))
            });
            return TypedResults.Ok(response);
        }
    }
}
