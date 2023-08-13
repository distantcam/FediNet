using System.Xml.Serialization;

namespace FediNet.Features.WellKnown;

public record Link(
    [property: XmlAttribute("rel")]
    string Rel,
    [property: XmlIgnore]
    string? Type,
    [property: XmlIgnore]
    string? Href,
    [property: XmlAttribute("template")]
    string? Template)
{
    private Link() : this(string.Empty, null, null, null) { }

    public static Link Create(string rel, string type, string href) => new Link(rel, type, href, null);
    public static Link Create(string href, string rel) => new Link(href, null, rel, null);
    public static Link CreateXrdXml(string template) => new Link("lrdd", null, null, template);
}
