namespace FediNet.Models.ActivityStreams;

public record Actor : ActivityObject
{
    public string? Inbox { get; init; }
    public string? Outbox { get; init; }

    ///<remarks>Required by Mastodon</remarks>
    public string? PreferredUsername { get; init; }

    // Extensions
    public PublicKey? PublicKey { get; init; }

}

public record PublicKey(string Id, string Owner, string PublicKeyPem);
