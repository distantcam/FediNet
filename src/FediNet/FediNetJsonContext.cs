using System.Text.Json.Serialization;
using FediNet.ActivityStreams;
using FediNet.Features.WellKnown;

namespace FediNet;

[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]

[JsonSerializable(typeof(FediNet.Features.WellKnown.Link))]
[JsonSerializable(typeof(NodeInfo.Response))]
[JsonSerializable(typeof(NodeInfoV20.Response))]

[JsonSerializable(typeof(ObjectOrLink))]
[JsonSerializable(typeof(List<ObjectOrLink?>))]

[JsonSerializable(typeof(Context))]
[JsonSerializable(typeof(ObjectActivityContext))]

[JsonSerializable(typeof(ActivityStreams.Link))]
[JsonSerializable(typeof(ObjectLink))]

[JsonSerializable(typeof(ASObject))]

[JsonSerializable(typeof(Collection))]
[JsonSerializable(typeof(OrderedCollection))]
[JsonSerializable(typeof(CollectionPage))]
[JsonSerializable(typeof(OrderedCollectionPage))]

[JsonSerializable(typeof(Activity))]
[JsonSerializable(typeof(Actor))]

[JsonSerializable(typeof(Question))]
[JsonSerializable(typeof(Closed))]
[JsonSerializable(typeof(DateTime))]
[JsonSerializable(typeof(bool))]

public partial class FediNetJsonContext : JsonSerializerContext
{
}
