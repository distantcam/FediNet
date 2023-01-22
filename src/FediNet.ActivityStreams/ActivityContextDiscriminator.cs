using System.Text.Json;
using System.Text.Json.Serialization;

namespace FediNet.ActivityStreams;

public class ActivityContextDiscriminator : JsonConverter<ActivityContext>
{
    public override bool CanConvert(Type typeToConvert) => typeof(ActivityContext).IsAssignableFrom(typeToConvert);

    public override ActivityContext? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return new StringActivityContext(reader.GetString()!);
        }

        //if (reader.TokenType == JsonTokenType.StartArray)
        //{
        //    return ReadArray(ref reader, typeToConvert, options);
        //}

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            return ReadObject(ref reader, options);
        }

        throw new JsonException();
    }

    //private ArrayActivityContext ReadArray(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //{
    //    var array = new ArrayActivityContext();
    //    while (reader.Read())
    //    {
    //        if (reader.TokenType == JsonTokenType.EndArray)
    //            break;

    //        var context = Read(ref reader, typeToConvert, options);

    //        if (context != null)
    //            array.Add(context);
    //    }
    //    return array;
    //}

    private ObjectActivityContext? ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var converter = (JsonConverter<ObjectActivityContext>)options.GetConverter(typeof(ObjectActivityContext));
        return converter.Read(ref reader, typeof(ObjectActivityContext), options);
    }

    public override void Write(Utf8JsonWriter writer, ActivityContext value, JsonSerializerOptions options)
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
