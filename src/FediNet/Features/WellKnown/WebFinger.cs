using FediNet.Infrastructure;
using Mediator;
using MMLib.MediatR.Generators.Controllers;

namespace FediNet.Features.WellKnown;

public static partial class WebFinger
{
    [HttpGet("/.well-known/webfinger", Controller = "WellKnown", From = From.Query)]
    public record Request(string Resource) : IRequest<Response>;

    public record Response(string Subject);

    [AutoConstruct]
    public partial class Handler : SyncRequestHandler<Request, Response>
    {
        public override Response Handle(Request request, CancellationToken cancellationToken)
        {
            var acct = new AcctUri(request.Resource);

            return new(acct.ToString());
        }
    }
}
