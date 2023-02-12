namespace FediNet.ActivityStreams;

public record Collection : ASObject
{
    public int? TotalItems { get; set; }

    public virtual IEnumerable<ObjectOrLink>? Items { get; set; }
}

public record OrderedCollection : ASObject
{
    public int? TotalItems { get; set; }

    public IEnumerable<ObjectOrLink>? OrderedItems { get; set; }
}
