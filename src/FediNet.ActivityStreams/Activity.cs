namespace FediNet.ActivityStreams;

public record Activity : ASObject
{
    public ObjectOrLink? Actor { get; set; }

    public ObjectOrLink? Target { get; set; }

    public ObjectOrLink? Origin { get; set; }

    // Not on IntransitiveActivities
    public ObjectOrLink? Object { get; set; }
}
