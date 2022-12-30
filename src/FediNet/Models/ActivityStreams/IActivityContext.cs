using System.Text.Json;
using System.Text.Json.Serialization;

namespace FediNet.Models.ActivityStreams;

[JsonConverter(typeof(ActivityContextDiscriminator))]
public interface IActivityContext { }

public struct StringActivityContext : IActivityContext
{
    public StringActivityContext(string context) => Context = context;
    public string Context { get; }
    public static implicit operator string(StringActivityContext context) => context.Context;
    public static implicit operator StringActivityContext(string context) => new(context);
}

public class ActivityContextDiscriminator : JsonConverter<IActivityContext>
{
    public override bool CanConvert(Type typeToConvert) => typeof(IActivityContext).IsAssignableFrom(typeToConvert);

    public override IActivityContext? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return new StringActivityContext(reader.GetString()!);
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, IActivityContext value, JsonSerializerOptions options)
    {
        if (value is StringActivityContext stringActivityContext)
        {
            writer.WriteStringValue(stringActivityContext.Context);
        }
        else
        {
            throw new JsonException();
        }
    }
}
