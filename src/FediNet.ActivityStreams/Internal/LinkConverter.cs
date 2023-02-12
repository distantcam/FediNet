using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Text.Json.JsonSerializer;

namespace FediNet.ActivityStreams.Internal;

internal class LinkConverter : JsonConverter<Link>
{
    public override Link? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var s = reader.GetString();
            if (!Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out _))
                throw new JsonException($"'{s}' could not be parsed as a Uri.");
            return s;
        }
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var converter = (JsonConverter<ObjectLink>)options.GetConverter(typeof(ObjectLink));
            return converter.Read(ref reader, typeof(ObjectLink), options);
        }
        throw new JsonException("Unable to parse Link.");
    }
    public override void Write(Utf8JsonWriter writer, Link value, JsonSerializerOptions options)
    {
        value.Switch(
            x => writer.WriteRawValue(Serialize(x, options)),
            x => writer.WriteStringValue(x)
        );
    }
}
