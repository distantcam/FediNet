using System.Text.Json.Serialization;

namespace FediNet.ActivityStreams;

public record Link : IObjectOrLink
{
    [JsonPropertyName("@context")]
    public Context? Context { get; set; }

    public string? Type { get; set; }

    public string? Href { get; set; }
    [JsonPropertyName("hreflang")] public string? HrefLang { get; set; }
    public string? MediaType { get; set; }
    public string? Name { get; set; }

    public string? Rel { get; set; }
}

public readonly struct StringLink : IObjectOrLink
{
    public StringLink(string value) => Value = value;
    public string Value { get; }
    public static implicit operator string(StringLink link) => link.Value;
    public static implicit operator StringLink(string value) => new(value);
}
