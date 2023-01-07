using Mediator;

namespace FediNet.Extensions;

public static class MediateExtensions
{
    public static RouteHandlerBuilder MediateGet<TRequest>(
        this IEndpointRouteBuilder app,
        string pattern) where TRequest : IRequest<IResult>
    {
        var requestType = typeof(TRequest)!;
        if (requestType.IsNested && requestType.DeclaringType != null)
            requestType = requestType.DeclaringType;

        return app.MapGet(pattern, async (ISender sender, [AsParameters] TRequest request) => await sender.Send(request))
            .WithTags(requestType.Namespace?.Split('.').Last() ?? nameof(FediNet));
    }

    public static RouteHandlerBuilder MediatePost<TRequest>(
        this IEndpointRouteBuilder app,
        string pattern) where TRequest : IRequest<IResult>
    {
        var requestType = typeof(TRequest)!;
        if (requestType.IsNested && requestType.DeclaringType != null)
            requestType = requestType.DeclaringType;

        return app.MapPost(pattern, async (ISender sender, [AsParameters] TRequest request) => await sender.Send(request))
            .WithTags(requestType.Namespace?.Split('.').Last() ?? nameof(FediNet));
    }
}
