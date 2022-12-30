namespace FediNet.Models.ActivityStreams;

public record Actor : ActivityObject
{
    public required string Inbox { get; init; }
    public required string Outbox { get; init; }

    ///<remarks>Required by Mastodon</remarks>
    public string? PreferredUsername { get; init; }
}
