using Mediator;

namespace Microsoft.AspNetCore.Routing;

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder MapGetRequest<T>(this IEndpointRouteBuilder endpoints, string pattern) where T : new()
    {
        return endpoints.MapGet(pattern, async (ISender sender) => await sender.Send(new T()));
    }
}
