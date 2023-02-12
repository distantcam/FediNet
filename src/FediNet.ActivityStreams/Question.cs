using System.Text.Json.Serialization;
using FediNet.ActivityStreams.Internal;
using OneOf;

namespace FediNet.ActivityStreams;

public record Question : Activity
{
    public ObjectOrLink? OneOf { get; set; }
    public ObjectOrLink? AnyOf { get; set; }
    public Closed? Closed { get; set; }
}

[GenerateOneOf]
[JsonConverter(typeof(ClosedConverter))]
public partial class Closed : OneOfBase<ObjectOrLink, DateTime, bool>
{
    internal Closed() : base(default) { }
}
