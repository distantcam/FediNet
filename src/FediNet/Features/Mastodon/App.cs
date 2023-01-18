using FediNet.Extensions;
using FediNet.Infrastructure;

namespace FediNet.Features.Mastodon;

public partial class App : Feature, IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder e) => e
        .MediatePostBody<Request>("/api/v1/apps");

    public record Request(
        string client_name,
        string redirect_uris,
        string? scopes,
        string? website
    ) : IFeatureRequest;

    public partial class Handler : SyncFeatureHandler<Request>
    {
        protected override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            return TypedResults.NotFound();
        }
    }
}
