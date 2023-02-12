namespace FediNet.ActivityStreams;

public record Question : Activity
{
    public IObjectOrLink? OneOf { get; set; }
    public IObjectOrLink? AnyOf { get; set; }
}
