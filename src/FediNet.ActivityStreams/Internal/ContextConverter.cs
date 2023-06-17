using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Text.Json.JsonSerializer;

namespace FediNet.ActivityStreams.Internal;

public class ContextConverter : JsonConverter<Context>
{
    public override bool CanConvert(Type typeToConvert) => typeof(Context).IsAssignableFrom(typeToConvert);

    public override Context? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
            var converter = (JsonConverter<ObjectActivityContext>)options.GetConverter(typeof(ObjectActivityContext));
            return converter.Read(ref reader, typeof(ObjectActivityContext), options);
        }
        throw new JsonException("Unable to parse Context.");
    }

    public override void Write(Utf8JsonWriter writer, Context value, JsonSerializerOptions options)
    {
        value.Switch(
            x => writer.WriteRawValue(Serialize(x, options)),
            x => writer.WriteStringValue(x)
        );
    }
}
