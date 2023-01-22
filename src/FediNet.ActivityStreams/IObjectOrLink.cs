using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Text.Json.JsonSerializer;

namespace FediNet.ActivityStreams;

[JsonConverter(typeof(ObjectOrLinkDescriminator))]
public interface IObjectOrLink
{
}

public class ObjectOrLinkDescriminator : JsonConverter<IObjectOrLink>
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
        { "Question", typeof(Activity) },
        { "Reject", typeof(Activity) },
        { "Read", typeof(Activity) },
        { "Remove", typeof(Activity) },
        { "TenativeReject", typeof(Activity) },
        { "TentativeAccept", typeof(Activity) },
        { "Travel", typeof(Activity) },
        { "Undo", typeof(Activity) },
        { "Update", typeof(Activity) },
        { "View", typeof(Activity) },

        { "Group", typeof(Actor) },
        { "Person", typeof(Actor) },
    };

    public override IObjectOrLink? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (JsonDocument.TryParseValue(ref reader, out JsonDocument? doc))
        {
            if (doc.RootElement.ValueKind is JsonValueKind.String)
            {
                return new StringLink(doc.RootElement.GetString()!);
            }
            else if (doc.RootElement.TryGetProperty("type", out var type))
            {
                var typeKey = type.GetString();
                if (typeKey == null || !TypeMap.TryGetValue(typeKey, out var clrType))
                {
                    throw new JsonException($"Unknown ActivityStream type '{typeKey}' on activity object.");
                }
                return (IObjectOrLink?)doc.Deserialize(clrType, options);
            }
        }

        throw new JsonException("Unable to read json.");
    }

    public override void Write(Utf8JsonWriter writer, IObjectOrLink value, JsonSerializerOptions options)
    {
        if (value is Link link) writer.WriteRawValue(Serialize(link, options));
        else if (value is Activity activity) writer.WriteRawValue(Serialize(activity, options));
        else if (value is CollectionPage collectionPage) writer.WriteRawValue(Serialize(collectionPage, options));
        else if (value is OrderedCollectionPage orderedCollectionPage) writer.WriteRawValue(Serialize(orderedCollectionPage, options));
        else if (value is Collection collection) writer.WriteRawValue(Serialize(collection, options));
        else if (value is OrderedCollection orderedCollection) writer.WriteRawValue(Serialize(orderedCollection, options));
        else if (value is Actor actor) writer.WriteRawValue(Serialize(actor, options));
        else if (value is ASObject obj) writer.WriteRawValue(Serialize(obj, options));
        else if (value is StringLink slink) writer.WriteRawValue("\"" + slink.Value + "\"");
        else throw new JsonException("Unknown type to write: " + value.GetType());
    }
}
