using System.Xml.Serialization;
using Microsoft.AspNetCore.WebUtilities;

namespace FediNet.Infrastructure;

public class XmlResult<TValue> : IResult, IStatusCodeHttpResult, IValueHttpResult, IValueHttpResult<TValue>, IContentTypeHttpResult
{
    private static readonly XmlSerializer _serializer = new(typeof(TValue));

    public XmlResult(TValue? value, string? contentType = null, int? statusCode = null)
    {
        Value = value;
        StatusCode = statusCode;
        ContentType = contentType;
    }

    public TValue? Value { get; }
    object? IValueHttpResult.Value => Value;

    public string? ContentType { get; }

    public int? StatusCode { get; }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        var ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        using var ms = new FileBufferingWriteStream();
        _serializer.Serialize(ms, Value, ns);

        httpContext.Response.StatusCode = StatusCode ?? 200;
        httpContext.Response.ContentType = ContentType ?? "application/xml";
        await ms.DrainBufferAsync(httpContext.Response.Body);
    }
}
