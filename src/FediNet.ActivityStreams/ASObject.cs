using System.Text.Json.Serialization;

namespace FediNet.ActivityStreams;

public record ASObject
{
    [JsonPropertyName("@context")]
    public Context? Context { get; set; }

    public string? Type { get; set; }
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Url { get; set; }
    public string? Summary { get; set; }
    public string? Content { get; set; }

    public ObjectOrLink? Location { get; set; }
}
