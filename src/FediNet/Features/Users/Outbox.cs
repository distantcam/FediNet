using AutoCtor;
using EndpointConfigurator;
using FediNet.Extensions;
using FediNet.Infrastructure;
using FediNet.Models.ActivityStreams;
using FediNet.Services;
using Mediator;

namespace FediNet.Features.Users;

public static partial class Outbox
{
    [EndpointConfig]
    public static void Config(IEndpointRouteBuilder app) =>
        app.MediateGet<Request>("/users/{username}/outbox")
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

public static partial class OutboxPage
{
    [EndpointConfig]
    public static void Config(IEndpointRouteBuilder app) =>
        app.MapGet("/users/{username}/outbox/{page}", () => Results.StatusCode(501))
            .WithName(nameof(OutboxPage));
}
