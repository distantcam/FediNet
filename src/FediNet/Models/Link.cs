namespace FediNet.Models;

public record Link(string Rel, string? Type, string? Href, string? Template)
{
    public static Link Create(string rel, string type, string href) => new Link(rel, type, href, null);
}
