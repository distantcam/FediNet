namespace FediNet.ActivityStreams;

public record CollectionPage : Collection
{
    public string? PartOf { get; set; }
}

public record OrderedCollectionPage : OrderedCollection
{
    public string? PartOf { get; set; }
}
