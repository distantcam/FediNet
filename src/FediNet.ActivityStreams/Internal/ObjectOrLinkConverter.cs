using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Text.Json.JsonSerializer;

namespace FediNet.ActivityStreams.Internal;

internal class ObjectOrLinkConverter : JsonConverter<ObjectOrLink>
{
    private static Dictionary<string, Type> TypeMap = new Dictionary<string, Type>()
    {
        { "Link", typeof(Link) },

        { "Object", typeof(ASObject) },
        { "Article", typeof(ASObject) },
        { "Audio", typeof(ASObject) },
        { "Document", typeof(ASObject) },
        { "Event", typeof(ASObject) },
        { "Image", typeof(ASObject) },
        { "Note", typeof(ASObject) },
        { "Page", typeof(ASObject) },
        { "Place", typeof(ASObject) },
        { "Profile", typeof(ASObject) },
        { "Relationship", typeof(ASObject) },
        { "Tombstone", typeof(ASObject) },
        { "Video", typeof(ASObject) },

        { "Collection", typeof(Collection) },
        { "OrderedCollection", typeof(OrderedCollection) },
        { "CollectionPage", typeof(CollectionPage) },
        { "OrderedCollectionPage", typeof(OrderedCollectionPage) },

        { "Activity", typeof(Activity) },
        { "Accept", typeof(Activity) },
        { "Add", typeof(Activity) },
        { "Announce", typeof(Activity) },
        { "Arrive", typeof(Activity) },
        { "Block", typeof(Activity) },
        { "Create", typeof(Activity) },
        { "Delete", typeof(Activity) },
        { "Dislike", typeof(Activity) },
        { "Flag", typeof(Activity) },
        { "Follow", typeof(Activity) },
        { "Ignore", typeof(Activity) },
        { "Invite", typeof(Activity) },
        { "Join", typeof(Activity) },
        { "Leave", typeof(Activity) },
        { "Like", typeof(Activity) },
        { "Listen", typeof(Activity) },
        { "Move", typeof(Activity) },
        { "Offer", typeof(Activity) },
        { "Reject", typeof(Activity) },
        { "Read", typeof(Activity) },
        { "Remove", typeof(Activity) },
        { "TentativeAccept", typeof(Activity) },
        { "TentativeReject", typeof(Activity) },
        { "Travel", typeof(Activity) },
        { "Undo", typeof(Activity) },
        { "Update", typeof(Activity) },
        { "View", typeof(Activity) },

        { "Group", typeof(Actor) },
        { "Person", typeof(Actor) },

        { "Question", typeof(Question) },
    };

    private Type GetClrTypeFromObject(JsonElement type)
    {
        var typeKey = type.GetString();
        if (typeKey == null || !TypeMap.TryGetValue(typeKey, out var clrType))
        {
            // Default to ASObject
            clrType = typeof(ASObject);
        }
        return clrType;
    }

    private ObjectOrLink? Cast(object? o)
    {
        ArgumentNullException.ThrowIfNull(o);
        if (o is ASObject obj) return obj;
        if (o is Link link) return link;
        throw new InvalidCastException($"Cannot cast {o.GetType()} to ObjectOrLink.");
    }

    public override ObjectOrLink? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (JsonDocument.TryParseValue(ref reader, out var doc))
        {
            if (doc.RootElement.ValueKind is JsonValueKind.String)
            {
                var s = doc.RootElement.GetString();
                if (!Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out _))
                    throw new JsonException($"'{s}' could not be parsed as a Uri.");
                return new Link(s);
            }
            else if (doc.RootElement.ValueKind is JsonValueKind.Array)
            {
                return doc.RootElement.EnumerateArray().Select(element =>
                {
                    if (!element.TryGetProperty("type", out var type))
                    {
                        throw new JsonException("Missing type property on object");
                    }
                    var clrType = GetClrTypeFromObject(type);
                    var result = element.Deserialize(clrType, options);
                    return Cast(result);
                }).ToList();
            }
            else if (doc.RootElement.TryGetProperty("type", out var type))
            {
                var clrType = GetClrTypeFromObject(type);
                var result = doc.Deserialize(clrType, options);
                return Cast(result);
            }
        }
        throw new JsonException("Unable to parse ObjectOrLink.");
    }

    public override void Write(Utf8JsonWriter writer, ObjectOrLink value, JsonSerializerOptions options)
    {
        value.Switch(
            o => writer.WriteRawValue(Serialize(o, o.GetType(), options)),
            o => writer.WriteRawValue(Serialize(o, options)),
            o => writer.WriteRawValue(Serialize(o, options))
        );
    }
}
