using AutoCtor;
using EndpointConfigurator;
using FediNet.Extensions;
using FediNet.Infrastructure;

namespace FediNet.Features.ActivityPub;

public static partial class User
{
    public record Request(string Username) : IHttpRequest;

    [AutoConstruct]
    public partial class Handler : SyncHttpRequestHandler<Request>
    {
        [EndpointConfig]
        public static void Config(IEndpointRouteBuilder app) =>
            app.MediateGet<Request>("/users/{username}")
                .WithName("UserPage");

        public override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            return Results.NoContent();
        }
    }
}
