using System.Text.Json.Serialization;
using FediNet.ActivityStreams.Internal;
using OneOf;

namespace FediNet.ActivityStreams;

[GenerateOneOf]
[JsonConverter(typeof(ContextConverter))]
public partial class Context : OneOfBase<ObjectActivityContext, string> { }

public record ObjectActivityContext
{
}
