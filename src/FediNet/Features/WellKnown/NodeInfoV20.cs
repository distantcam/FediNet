using System.Reflection;
using AutoCtor;
using EndpointConfigurator;
using FediNet.Extensions;
using FediNet.Infrastructure;

namespace FediNet.Features.WellKnown;

public static partial class NodeInfoV20
{
    public record Request : IHttpRequest;

    public record Response(string Version, Software Software, string[] Protocols, Services Services, bool OpenRegistrations, Usage Usage, Metadata Metadata);

    public record Software(string Name, string Version);

    public record Services(string[] Inbound, string[] Outbound);

    public record Usage(UserCounts Users, int LocalPosts, int LocalComments);

    public record UserCounts(int Total, int ActiveHalfyear, int ActiveMonth);

    public record Metadata;

    [AutoConstruct]
    public partial class Handler : SyncHttpRequestHandler<Request>
    {
        [EndpointConfig]
        public static void Config(IEndpointRouteBuilder app) =>
            app.MediateGet<Request>("/nodeinfo/2.0.json")
                .WithName("NodeInfoV2");

        public override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            var assemblyName = Assembly.GetEntryAssembly()?.GetName();
            var software = assemblyName == null
                ? new Software("fedinet", "0.0.0")
                : new Software(assemblyName.Name?.ToLowerInvariant() ?? "fedinet", assemblyName.Version?.ToString(3) ?? "0.0.0");

            var response = new Response(
                "2.0",
                software,
                new[] { "activitypub" },
                new Services(Array.Empty<string>(), Array.Empty<string>()),
                false,
                new Usage(new UserCounts(0, 0, 0), 0, 0),
                new Metadata());

            return Results.Ok(response);
        }
    }
}
