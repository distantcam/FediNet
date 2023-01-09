using AutoCtor;
using FediNet.Extensions;
using FediNet.Infrastructure;
using FediNet.Services;
using Mediator;

namespace FediNet.Features.WellKnown;

public partial class NodeInfo : IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .MediateGet<Request>("/.well-known/nodeinfo")
        .Produces<Response>(StatusCodes.Status200OK);

    public record Request : IRequest<IResult>;

    public record Response(IEnumerable<Link> Links);

    public record Link(string Href, string Rel);

    [AutoConstruct]
    public partial class Handler : SyncRequestHandler<Request, IResult>
    {
        private readonly UriGenerator _uriGenerator;

        protected override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            var response = new Response(new[]
            {
                new Link("http://nodeinfo.diaspora.software/ns/schema/2.0",
                _uriGenerator.GetUriByName(nameof(NodeInfoV20)))
            });
            return Results.Ok(response);
        }
    }
}
