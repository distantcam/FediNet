namespace FediNet.ActivityStreams;

public record Activity : ASObject
{
    public IObjectOrLink? Actor { get; set; }

    public IObjectOrLink? Target { get; set; }

    public IObjectOrLink? Origin { get; set; }

    // Not on IntransitiveActivities
    public IObjectOrLink? Object { get; set; }
}
