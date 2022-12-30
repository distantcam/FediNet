namespace FediNet.Models.ActivityStreams;

public record ActivityCollection : ActivityObject
{
    public int TotalItems { get; set; }
    public string? Current { get; set; }
    public string? First { get; set; }
    public string? Last { get; set; }
    public IEnumerable<IActivityObjectOrLink>? Items { get; set; }
}

public record ActivityOrderedCollection : ActivityCollection
{
    public IOrderedEnumerable<IActivityObjectOrLink>? OrderedItems
    {
        get => Items?.Order();
        set => Items = value;
    }
}
