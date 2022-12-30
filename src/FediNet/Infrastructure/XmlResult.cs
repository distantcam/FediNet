using System.Xml.Serialization;
using Microsoft.AspNetCore.WebUtilities;

namespace FediNet.Infrastructure;

public class XmlResult<T> : IResult
{
    private static readonly XmlSerializer _serializer = new(typeof(T));

    private readonly T _result;
    private readonly string _contentType;
    private readonly int _statusCode;

    public XmlResult(T result, string? contentType = null, int? statusCode = null)
    {
        _result = result;
        _contentType = contentType ?? "application/xml";
        _statusCode = statusCode ?? 200;
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        var ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        using var ms = new FileBufferingWriteStream();
        _serializer.Serialize(ms, _result, ns);

        httpContext.Response.ContentType = _contentType;
        httpContext.Response.StatusCode = _statusCode;
        await ms.DrainBufferAsync(httpContext.Response.Body);
    }
}

public static class XmlResultExtensions
{
    public static IResult Xml<T>(this IResultExtensions _, T result, string? contentType = null, int? statusCode = null) =>
        new XmlResult<T>(result, contentType, statusCode);
}
