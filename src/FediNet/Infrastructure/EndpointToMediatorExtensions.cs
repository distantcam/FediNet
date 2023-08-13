using System.Diagnostics.CodeAnalysis;
using Mediator;

namespace Microsoft.AspNetCore.Routing;

// TODO Generate these
public static class EndpointToMediatorExtensions
{
    public static RouteHandlerBuilder SendGet<TRequest, TResponse, TResult>(
        this IEndpointRouteBuilder endpoints,
        [StringSyntax("Route")] string pattern,
        Func<TRequest> newRequest,
        Func<TResponse, TResult> resultMap
    ) where TRequest : IRequest<TResponse>
    {
        return endpoints.MapGet(pattern, async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(newRequest(), cancellationToken);
            return resultMap(response);
        });
    }

    public static RouteHandlerBuilder SendGet<TRequest, TResponse, TResult>(
        this IEndpointRouteBuilder endpoints,
        [StringSyntax("Route")] string pattern,
        Func<string, TRequest> newRequest,
        Func<TResponse, TResult> resultMap
    ) where TRequest : IRequest<TResponse>
    {
        return endpoints.MapGet(pattern, async (string resource, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(newRequest(resource), cancellationToken);
            return resultMap(response);
        });
    }
}
