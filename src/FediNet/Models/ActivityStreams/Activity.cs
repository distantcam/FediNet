using System.Text.Json.Serialization;

namespace FediNet.Models.ActivityStreams;

public record ActivityObject : IActivityObjectOrLink
{
    [JsonPropertyName("@context")]
    public required IActivityContext Context { get; init; }

    public string? Id { get; init; }
    public string? Type { get; init; }

    public string? Name { get; set; }
}

public record ActivityLink : IActivityObjectOrLink
{
    public string? Href { get; init; }
    public string? Rel { get; init; }
    public string? MediaType { get; init; }
    public string? Name { get; init; }
    [JsonPropertyName("hreflang")]
    public string? HrefLang { get; init; }
    public string? Height { get; init; }
    public string? Width { get; init; }
    public string? Preview { get; init; }
}

public interface IActivityObjectOrLink
{
}
