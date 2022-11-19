using Mediator;

namespace FediNet.Features.WellKnown;

public static partial class NodeInfo
{
    public record Query : IRequest<Response>;

    public record Response(IEnumerable<Link> Links);

    public record Link(string Href, string Rel);

    [AutoConstruct]
    public partial class Handler : IRequestHandler<Query, Response>
    {
        public ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(new Response(new[]
            {
                new Link("http://nodeinfo.diaspora.software/ns/schema/2.0", "https://localhost/nodeinfo/2.0.json"),
                new Link("http://nodeinfo.diaspora.software/ns/schema/2.1", "https://localhost/nodeinfo/2.1.json")
            }));
        }
    }
}
