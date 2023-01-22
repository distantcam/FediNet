using System.Text.Json;
using System.Text.Json.Serialization;

namespace FediNet.ActivityStreams;

public record ActivityLink : IActivityObjectOrLink
{
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtraProperties { get; set; }
}
