using System.Reflection;
using AutoCtor;
using FediNet.Extensions;
using FediNet.Infrastructure;
using Mediator;

namespace FediNet.Features.WellKnown;

public partial class NodeInfoV20 : IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .MediateGet<Request>("/nodeinfo/2.0.json")
        .Produces<Response>(StatusCodes.Status200OK)
        .WithName(nameof(NodeInfoV20));

    public record Request : IRequest<IResult>;

    public record Response(string Version, Software Software, string[] Protocols, Services Services, bool OpenRegistrations, Usage Usage, Metadata Metadata);

    public record Software(string Name, string Version);

    public record Services(string[] Inbound, string[] Outbound);

    public record Usage(UserCounts Users, int LocalPosts, int LocalComments);

    public record UserCounts(int Total, int ActiveHalfyear, int ActiveMonth);

    public record Metadata;

    [AutoConstruct]
    public partial class Handler : SyncRequestHandler<Request, IResult>
    {
        protected override IResult Handle(Request request, CancellationToken cancellationToken)
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
