using AutoCtor;
using EndpointConfigurator;
using FediNet.Extensions;
using FediNet.Infrastructure;
using FediNet.Services;

namespace FediNet.Features.WellKnown;

public static partial class NodeInfo
{
    public record Request : IHttpRequest;

    public record Response(IEnumerable<Link> Links);

    public record Link(string Href, string Rel);

    [AutoConstruct]
    public partial class Handler : SyncHttpRequestHandler<Request>
    {
        [EndpointConfig]
        public static void Config(IEndpointRouteBuilder app) =>
            app.MediateGet<Request>("/.well-known/nodeinfo");

        private readonly UriGenerator _uriGenerator;

        public override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            var response = new Response(new[]
            {
                new Link("http://nodeinfo.diaspora.software/ns/schema/2.0",
                _uriGenerator.GetUriByName("NodeInfoV2", null)!)
            });
            return Results.Ok(response);
        }
    }
}
