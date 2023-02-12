using System.Text.Json.Serialization;
using FediNet.ActivityStreams.Internal;
using OneOf;

namespace FediNet.ActivityStreams;

[GenerateOneOf]
[JsonConverter(typeof(ObjectOrLinkConverter))]
public partial class ObjectOrLink : OneOfBase<ASObject, Link, List<ObjectOrLink>>
{
    internal ObjectOrLink() : base(default) { }
}
