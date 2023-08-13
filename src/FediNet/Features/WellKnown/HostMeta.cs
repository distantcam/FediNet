using System.Web;
using System.Xml.Serialization;
using AutoCtor;
using FediNet.Services;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FediNet.Features.WellKnown;

public partial class HostMeta : IEndpointGroupDefinition
{
    private static readonly XmlSerializer _serializer = new(typeof(Response));
    public static void MapEndpoint(RouteGroupBuilder builder) => builder
        .SendGet<Request, Response, ContentHttpResult>(
            "/.well-known/host-meta",
            () => new Request(),
            response =>
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                using var writer = new StringWriter();
                _serializer.Serialize(writer, response, ns);
                return TypedResults.Text(writer.ToString(), contentType: "application/xrd+xml");
            });

    public record Request : IRequest<Response>;
    [XmlRoot("XRD", Namespace = "http://docs.oasis-open.org/ns/xri/xrd-1.0")]
    public record Response
    {
        public required Link Link { get; init; }
    }

    [AutoConstruct]
    public partial class Handler : SyncHandler<Request, Response>
    {
        private readonly UriGenerator _uriGenerator;

        protected override Response Handle(Request request)
        {
            var response = new Response
            {
                Link = Link.CreateXrdXml(HttpUtility.UrlDecode(_uriGenerator.GetUriByName(nameof(WebFinger), new { resource = "{uri}" })))
            };
            return response;
        }
    }
}
