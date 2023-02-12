namespace FediNet.ActivityStreams;

public record Question : Activity
{
    public ObjectOrLink? OneOf { get; set; }
    public ObjectOrLink? AnyOf { get; set; }
}
