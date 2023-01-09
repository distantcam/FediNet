using AutoCtor;
using FediNet.Extensions;
using FediNet.Infrastructure;
using FediNet.Models.ActivityStreams;
using FediNet.Services;
using Mediator;

namespace FediNet.Features.Users;

public partial class Outbox : IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .MediateGet<Request>("/users/{username}/outbox")
        .RequireAuthorization()
        .WithName(nameof(Outbox));

    public record Request(string Username) : IRequest<IResult>;

    public record Response;

    [AutoConstruct]
    public partial class Handler : SyncRequestHandler<Request, IResult>
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
            return Results.Extensions.JsonActivity(orderedCollection);
        }
    }
}

public partial class OutboxPage
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .MapGet("/users/{username}/outbox/{page}", () => Results.StatusCode(501))
        .WithName(nameof(OutboxPage));
}
