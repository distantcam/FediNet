namespace FediNet.ActivityStreams;

public class ObjectOrLinkList : List<ObjectOrLink?>
{
    public ObjectOrLinkList()
    {
    }

    public ObjectOrLinkList(IEnumerable<ObjectOrLink?> collection) : base(collection)
    {
    }
}
