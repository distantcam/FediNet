using FediNet.Services;
using System.Web;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FediNet.Features.WellKnown;

public class HostMeta : IEndpointGroupDefinition
{
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .MapGet("/.well-known/host-meta", Handler);

    private static readonly XmlSerializer _serializer = new(typeof(Response));

    [XmlRoot("XRD", Namespace = "http://docs.oasis-open.org/ns/xri/xrd-1.0")]
    public record Response
    {
        public required Link Link { get; init; }
    }

    public record Link
    {
        [XmlAttribute("rel")]
        public string Rel { get; set; } = "lrdd";

        [XmlAttribute("template")]
        public required string Template { get; init; }
    }

    private static ContentHttpResult Handler(UriGenerator uriGenerator)
    {
        var response = new Response
        {
            Link = new()
            {
                Template = HttpUtility.UrlDecode(uriGenerator.GetUriByName(nameof(WebFinger), new { resource = "{uri}" }))
            }
        };

        var ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        using var writer = new StringWriter();
        _serializer.Serialize(writer, response, ns);

        return TypedResults.Text(writer.ToString(), contentType: "application/xrd+xml");
    }
}
