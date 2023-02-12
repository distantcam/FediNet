using System.Text.Json.Serialization;

namespace FediNet.ActivityStreams;

[JsonConverter(typeof(ObjectOrLinkDescriminator))]
public interface IObjectOrLink
{
}
