using FediNet.Infrastructure;
using Mediator;

namespace FediNet.Extensions;

public static class MediateExtensions
{
    public static RouteHandlerBuilder MediateGet<TRequest>(
        this IEndpointRouteBuilder app,
        string pattern) where TRequest : IHttpRequest
    {
        return app.MapGet(pattern, async (ISender sender, [AsParameters] TRequest request) => await sender.Send(request));
    }

    public static RouteHandlerBuilder MediatePost<TRequest>(
        this IEndpointRouteBuilder app,
        string pattern) where TRequest : IHttpRequest
    {
        return app.MapPost(pattern, async (ISender sender, [AsParameters] TRequest request) => await sender.Send(request));
    }
}
