namespace FediNet.Features.WellKnown;

public record Link(string Rel, string? Type, string? Href, string? Template)
{
    public static Link Create(string rel, string type, string href) => new Link(rel, type, href, null);
    public static Link Create(string href, string rel) => new Link(href, null, rel, null);
}
