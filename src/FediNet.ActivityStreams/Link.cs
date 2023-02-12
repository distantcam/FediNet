using System.Text.Json.Serialization;
using FediNet.ActivityStreams.Internal;
using OneOf;

namespace FediNet.ActivityStreams;

[GenerateOneOf]
[JsonConverter(typeof(LinkConverter))]
public partial class Link : OneOfBase<ObjectLink, string> { }

public record ObjectLink
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
