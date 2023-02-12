using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Text.Json.JsonSerializer;

namespace FediNet.ActivityStreams.Internal;

internal class ClosedConverter : JsonConverter<Closed>
{
    public override Closed? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var localReader = reader;
            if (localReader.TryGetDateTime(out var dateTime)) return dateTime;
            var s = localReader.GetString();
            if (s != null)
            {
                if (s.Equals("true", StringComparison.InvariantCultureIgnoreCase) || s.Equals("1", StringComparison.InvariantCultureIgnoreCase))
                    return true;
                if (s.Equals("false", StringComparison.InvariantCultureIgnoreCase) || s.Equals("0", StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }
        }
        var converter = (JsonConverter<ObjectOrLink>)options.GetConverter(typeof(ObjectOrLink));
        return converter.Read(ref reader, typeof(ObjectOrLink), options);
    }
    public override void Write(Utf8JsonWriter writer, Closed value, JsonSerializerOptions options)
    {
        value.Switch(
            o => writer.WriteRawValue(Serialize(o, options)),
            o => writer.WriteRawValue(Serialize(o, options)),
            o => writer.WriteStringValue(o ? "true" : "false")
        );
    }
}
