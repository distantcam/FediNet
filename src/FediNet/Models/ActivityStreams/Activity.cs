using System.Text.Json;
using System.Text.Json.Serialization;

namespace FediNet.Models.ActivityStreams;

public interface IActivityObjectOrLink
{
}

public record ActivityObject : IActivityObjectOrLink
{
    [JsonPropertyName("@context")]
    public required IActivityContext Context { get; init; }

    public string? Id { get; init; }
    public string? Type { get; init; }

    public string? Name { get; set; }
    public string? Url { get; set; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtraProperties { get; set; }
}

public record ActivityLink : IActivityObjectOrLink
{
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtraProperties { get; set; }
}
