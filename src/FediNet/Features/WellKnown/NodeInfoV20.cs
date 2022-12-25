﻿using System.Reflection;
using FediNet.Infrastructure;
using Mediator;
using MMLib.MediatR.Generators.Controllers;

namespace FediNet.Features.WellKnown;

public static partial class NodeInfoV20
{
    [HttpGet("/nodeinfo/2.0.json", Controller = "WellKnown", From = From.Ignore)]
    public record Request : IRequest<Response>;

    public record Response(string Version, Software Software, string[] Protocols, Services Services, bool OpenRegistrations, Usage Usage, Metadata Metadata);

    public record Software(string Name, string Version);

    public record Services(string[] Inbound, string[] Outbound);

    public record Usage(UserCounts Users, int LocalPosts, int LocalComments);

    public record UserCounts(int Total, int ActiveHalfyear, int ActiveMonth);

    public record Metadata;

    [AutoConstruct]
    public partial class Handler : SyncRequestHandler<Request, Response>
    {
        public override Response Handle(Request request, CancellationToken cancellationToken)
        {
            var assemblyName = Assembly.GetEntryAssembly()?.GetName();
            var software = assemblyName == null
                ? new Software("fedinet", "0.0.0")
                : new Software(assemblyName.Name?.ToLowerInvariant() ?? "fedinet", assemblyName.Version?.ToString(3) ?? "0.0.0");

            return new Response(
                "2.0",
                software,
                new[] { "activitypub" },
                new Services(Array.Empty<string>(), Array.Empty<string>()),
                false,
                new Usage(new UserCounts(0, 0, 0), 0, 0),
                new Metadata());
        }
    }
}
