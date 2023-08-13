using AutoCtor;
using Injectio.Attributes;
using KristofferStrube.ActivityStreams;

namespace FediNet.Services;

[AutoConstruct]
[RegisterSingleton]
public partial class UriGenerator
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    private (string scheme, HostString host) GetCurrentContext()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
            throw new InvalidOperationException("Not in HTTP context");

        var scheme = _configuration["scheme"] ?? httpContext.Request.Scheme;
        var hostConfig = _configuration["host"];
        var host = string.IsNullOrWhiteSpace(hostConfig) ? httpContext.Request.Host : new HostString(hostConfig);

        return (scheme, host);
    }

    public ILink GetLinkByName(string endpointName, object? values = null)
        => new Link { Href = new Uri(GetUriByName(endpointName, values)) };

    public string GetUriByName(string endpointName, object? values = null)
    {
        (var scheme, var host) = GetCurrentContext();

        return _linkGenerator.GetUriByName(endpointName, values, scheme, host) ?? throw new ArgumentException(endpointName, nameof(endpointName));
    }

    public string GetCurrentUri()
    {
        (var scheme, var host) = GetCurrentContext();

        var request = _httpContextAccessor.HttpContext?.Request;

        return scheme + Uri.SchemeDelimiter + host.Value + request?.PathBase.Value + request?.Path.Value + request?.QueryString.Value;
    }
}
