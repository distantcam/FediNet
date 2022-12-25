using FediNet.Infrastructure;
using Mediator;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using MMLib.MediatR.Generators.Controllers;

namespace FediNet.Features.WellKnown;

public static partial class NodeInfo
{
    [HttpGet("/.well-known/nodeinfo", Controller = "WellKnown", From = From.Ignore)]
    public record Request : IRequest<Response>;

    public record Response(IEnumerable<Link> Links);

    public record Link(string Href, string Rel);

    [AutoConstruct]
    public partial class Handler : SyncRequestHandler<Request, Response>
    {
        private readonly IServer _server;

        public override Response Handle(Request request, CancellationToken cancellationToken)
        {
            var address = _server.Features.Get<IServerAddressesFeature>()?.Addresses.FirstOrDefault() ?? "https://localhost";
            return new Response(new[]
            {
                new Link("http://nodeinfo.diaspora.software/ns/schema/2.0",
                $"{address}/nodeinfo/2.0.json")
            });
        }
    }
}
