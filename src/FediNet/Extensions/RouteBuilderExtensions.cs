using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace FediNet.Extensions;

public static class RouteBuilderExtensions
{
    public static RouteHandlerBuilder MediateGet<TRequest>(
        this IEndpointRouteBuilder builder,
        string pattern) where TRequest : IRequest<IResult>
    {
        var requestType = typeof(TRequest)!;
        if (requestType.IsNested && requestType.DeclaringType != null)
            requestType = requestType.DeclaringType;

        return builder.MapGet(pattern, (ISender sender, [AsParameters] TRequest request) => sender.Send(request))
            .WithTags(requestType.Namespace?.Split('.').Last() ?? nameof(FediNet));
    }

    public static RouteHandlerBuilder MediateGetBody<TRequest>(
        this IEndpointRouteBuilder builder,
        string pattern) where TRequest : IRequest<IResult>
    {
        var requestType = typeof(TRequest)!;
        if (requestType.IsNested && requestType.DeclaringType != null)
            requestType = requestType.DeclaringType;

        return builder.MapGet(pattern, (ISender sender, [FromBody] TRequest request) => sender.Send(request))
            .WithTags(requestType.Namespace?.Split('.').Last() ?? nameof(FediNet));
    }


    public static RouteHandlerBuilder MediatePost<TRequest>(
        this IEndpointRouteBuilder builder,
        string pattern) where TRequest : IRequest<IResult>
    {
        var requestType = typeof(TRequest)!;
        if (requestType.IsNested && requestType.DeclaringType != null)
            requestType = requestType.DeclaringType;

        return builder.MapPost(pattern, (ISender sender, [AsParameters] TRequest request) => sender.Send(request))
            .WithTags(requestType.Namespace?.Split('.').Last() ?? nameof(FediNet));
    }

    public static RouteHandlerBuilder MediatePostBody<TRequest>(
        this IEndpointRouteBuilder builder,
        string pattern) where TRequest : IRequest<IResult>
    {
        var requestType = typeof(TRequest)!;
        if (requestType.IsNested && requestType.DeclaringType != null)
            requestType = requestType.DeclaringType;

        return builder.MapPost(pattern, (ISender sender, [FromBody] TRequest request) => sender.Send(request))
            .WithTags(requestType.Namespace?.Split('.').Last() ?? nameof(FediNet));
    }
}
