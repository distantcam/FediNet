using System.Text.Json.Serialization;

namespace FediNet.ActivityStreams;

[JsonConverter(typeof(ActivityContextDiscriminator))]
public abstract class ActivityContext
{
    public static implicit operator ActivityContext(string context) => new StringActivityContext(context);
}

public class StringActivityContext : ActivityContext
{
    public StringActivityContext(string context) => Context = context;
    public string Context { get; }
    public static implicit operator string(StringActivityContext context) => context.Context;
    public static implicit operator StringActivityContext(string context) => new(context);
}

public class ObjectActivityContext : ActivityContext
{
    [JsonExtensionData]
    public IDictionary<string, object>? ExtraProperties { get; set; }
}
