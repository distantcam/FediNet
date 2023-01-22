using System.Text.Json.Serialization;

namespace FediNet.ActivityStreams;

[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(IObjectOrLink))]
[JsonSerializable(typeof(ASObject))]
[JsonSerializable(typeof(Link))]
[JsonSerializable(typeof(Activity))]
[JsonSerializable(typeof(Collection))]
[JsonSerializable(typeof(OrderedCollection))]
[JsonSerializable(typeof(CollectionPage))]
[JsonSerializable(typeof(OrderedCollectionPage))]
[JsonSerializable(typeof(Actor))]
[JsonSerializable(typeof(StringLink))]
public partial class ActivityStreamsJsonContext : JsonSerializerContext
{
}
