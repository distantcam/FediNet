using AutoCtor;
using FediNet.Extensions;
using FediNet.Infrastructure;
using FediNet.Models.ActivityStreams;
using FediNet.Services;

namespace FediNet.Features.Users;

public partial class Outbox : Feature, IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .MediateGet<Request>("/users/{username}/outbox")
        .RequireAuthorization()
        .WithName(nameof(Outbox));

    public record Request(string Username) : IFeatureRequest;

    public record Response;

    [AutoConstruct]
    public partial class Handler : SyncFeatureHandler<Request>
    {
        private readonly UriGenerator _uriGenerator;

        protected override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            var orderedCollection = new ActivityOrderedCollection
            {
                Context = Constants.ActivityStreamsContext,
                Id = _uriGenerator.GetCurrentUri(),
                Type = "OrderedCollection",
                First = _uriGenerator.GetUriByName(nameof(OutboxPage), new { username = request.Username, page = 0 }),
                Last = _uriGenerator.GetUriByName(nameof(OutboxPage), new { username = request.Username, page = 0 }),
                TotalItems = 0
            };
            return TypedResults.Json(orderedCollection, contentType: "application/activity+json");
        }
    }
}

public partial class OutboxPage : IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .MapGet("/users/{username}/outbox/{page}", () => Results.StatusCode(501))
        .WithName(nameof(OutboxPage));
}
