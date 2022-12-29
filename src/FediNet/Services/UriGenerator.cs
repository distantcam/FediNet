using AutoCtor;

namespace FediNet.Services;

[AutoConstruct]
public partial class UriGenerator
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public string? GetUriByName(string endpointName, object? values)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
            throw new InvalidOperationException("Not in HTTP context");

        var scheme = _configuration["scheme"] ?? httpContext.Request.Scheme;
        var hostConfig = _configuration["host"];
        var host = string.IsNullOrWhiteSpace(hostConfig) ? httpContext.Request.Host : new HostString(hostConfig);

        return _linkGenerator.GetUriByName(endpointName, values, scheme, host);
    }
}
