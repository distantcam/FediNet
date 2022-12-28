using AutoCtor;
using FediNet.Infrastructure;

namespace FediNet.Features.Api;

public static partial class Apps
{
    public record Request : IHttpRequest;

    [AutoConstruct]
    public partial class Handler : SyncRequestHandler<Request, IResult>
    {
        public override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            return Results.Ok();
        }
    }
}
