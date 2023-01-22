using System.Text.Json;
using System.Text.Json.Serialization;

namespace FediNet.ActivityStreams;

public record ActivityObject : IActivityObjectOrLink
{
    [JsonPropertyName("@context")]
    public required ActivityContext Context { get; init; }

    public string? Id { get; init; }
    public string? Type { get; init; }

    public string? Name { get; set; }
    public string? Url { get; set; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtraProperties { get; set; }
}
