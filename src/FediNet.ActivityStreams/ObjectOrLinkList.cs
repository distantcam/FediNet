namespace FediNet.ActivityStreams;

public class ObjectOrLinkList : List<IObjectOrLink?>, IObjectOrLink
{
    public ObjectOrLinkList()
    {
    }

    public ObjectOrLinkList(IEnumerable<IObjectOrLink?> collection) : base(collection)
    {
    }
}
