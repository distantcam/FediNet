using FediNet.Models.ActivityStreams;

namespace FediNet;

public static class Constants
{
    public static IActivityContext ActivityStreamsContext = new StringActivityContext("https://www.w3.org/ns/activitystreams");

    public static class ContentTypes
    {
        public static string Activity = "application/activity+json";
    }
}
