using System.Text.Json;
using AutoCtor;
using FediNet.Models.ActivityStreams;

namespace FediNet.Services;

[AutoConstruct]
public partial class ActorHelper
{
    private readonly IHttpClientFactory _httpClientFactory;

    public async Task<Actor> GetActor(string actorId)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Accept", "application/activity+json");
        var result = await httpClient.GetAsync(actorId);

        result.EnsureSuccessStatusCode();
        var content = await result.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var actor = JsonSerializer.Deserialize<Actor>(content, options);
        return actor!;
    }
}
