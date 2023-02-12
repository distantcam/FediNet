using System.Text.Json.Serialization;

namespace FediNet.ActivityStreams;

[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]

[JsonSerializable(typeof(ObjectOrLink))]
[JsonSerializable(typeof(List<ObjectOrLink?>))]

[JsonSerializable(typeof(Context))]
[JsonSerializable(typeof(ObjectActivityContext))]

[JsonSerializable(typeof(Link))]
[JsonSerializable(typeof(ObjectLink))]

[JsonSerializable(typeof(ASObject))]

[JsonSerializable(typeof(Collection))]
[JsonSerializable(typeof(OrderedCollection))]
[JsonSerializable(typeof(CollectionPage))]
[JsonSerializable(typeof(OrderedCollectionPage))]

[JsonSerializable(typeof(Activity))]
[JsonSerializable(typeof(Actor))]
[JsonSerializable(typeof(Question))]
public partial class ActivityStreamsJsonContext : JsonSerializerContext { }
