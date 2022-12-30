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

    public string? Attachment { get; set; }
    public string? AttributedTo { get; set; }
    public string? Audience { get; set; }
    public string? Content { get; set; }
    public string? Name { get; set; }
    public string? EndTime { get; set; }
    public string? Generator { get; set; }
    public string? Icon { get; set; }
    public string? Image { get; set; }
    public string? InReplyTo { get; set; }
    public string? Location { get; set; }
    public string? Preview { get; set; }
    public string? Published { get; set; }
    public string? Replies { get; set; }
    public string? StartTime { get; set; }
    public string? Summary { get; set; }
    public string? Tag { get; set; }
    public string? Updated { get; set; }
    public string? Url { get; set; }
    public string? To { get; set; }
    public string? Bto { get; set; }
    public string? Cc { get; set; }
    public string? Bcc { get; set; }
    public string? MediaType { get; set; }
    public string? Duration { get; set; }
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
