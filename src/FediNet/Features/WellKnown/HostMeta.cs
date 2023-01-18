using System.Web;
using System.Xml.Serialization;
using AutoCtor;
using FediNet.Extensions;
using FediNet.Infrastructure;
using FediNet.Services;

namespace FediNet.Features.WellKnown;

public partial class HostMeta : Feature, IEndpointDefinition
{
    public static void MapEndpoint(IEndpointRouteBuilder builder) => builder
        .MediateGet<Request>("/.well-known/host-meta")
        .Produces<Response>(StatusCodes.Status200OK, contentType: "application/xrd+xml");

    public record Request : IFeatureRequest;

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
    public partial class Handler : SyncFeatureHandler<Request>
    {
        private static readonly XmlSerializer _serializer = new(typeof(Response));

        private readonly UriGenerator _uriGenerator;

        protected override IResult Handle(Request request, CancellationToken cancellationToken)
        {
            var response = new Response
            {
                Link = new()
                {
                    Template = HttpUtility.UrlDecode(_uriGenerator.GetUriByName(nameof(WebFinger), new { resource = "{uri}" }))
                }
            };

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using var writer = new StringWriter();
            _serializer.Serialize(writer, response, ns);

            return TypedResults.Text(writer.ToString(), contentType: "application/xrd+xml");
        }
    }
}
