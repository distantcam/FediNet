namespace FediNet.ActivityStreams;

public record Collection : ASObject
{
    public int? TotalItems { get; set; }

    public virtual IEnumerable<IObjectOrLink>? Items { get; set; }
}

public record OrderedCollection : ASObject
{
    public int? TotalItems { get; set; }

    public IEnumerable<IObjectOrLink>? OrderedItems { get; set; }
}
