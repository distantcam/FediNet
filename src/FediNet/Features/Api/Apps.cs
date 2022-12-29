using AutoCtor;
using FediNet.Infrastructure;

namespace FediNet.Features.Api;

public static partial class Apps
{
    public record Request : IHttpRequest;

    [AutoConstruct]
    public partial class Handler : SyncHttpRequestHandler<Request>
    {
        public override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            return Results.Ok();
        }
    }
}
