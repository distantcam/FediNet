using System.Web;
using System.Xml.Serialization;
using AutoCtor;
using EndpointConfigurator;
using FediNet.Extensions;
using FediNet.Infrastructure;
using FediNet.Services;

namespace FediNet.Features.WellKnown;

public static partial class HostMeta
{
    [EndpointConfig]
    public static void Config(IEndpointRouteBuilder app) =>
        app.MediateGet<Request>("/.well-known/host-meta")
            .Produces<Response>(StatusCodes.Status200OK, contentType: "application/xrd+xml");

    public record Request : IHttpRequest;

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

    [AutoConstruct]
    public partial class Handler : SyncHttpRequestHandler<Request>
    {
        private readonly UriGenerator _uriGenerator;

        public override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            return Results.Extensions.Xml(new Response
            {
                Link = new()
                {
                    Template = HttpUtility.UrlDecode(_uriGenerator.GetUriByName(nameof(WebFinger), new { resource = "{uri}" }))
                }
            }, contentType: "application/xrd+xml");
        }
    }
}
