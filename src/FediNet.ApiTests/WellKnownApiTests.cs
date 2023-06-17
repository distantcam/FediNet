namespace FediNet.ApiTests;

[UsesVerify]
public class WellKnownApiTests
{
    [Theory]
    [MemberData(nameof(GetEndpoints))]
    public async Task Get(string requestUri)
    {
        await using var application = new FediNetApplication();

        var client = application.CreateClient();
        var response = await client.GetAsync(requestUri);

        Assert.True(response.IsSuccessStatusCode);

        await Verify(response).UseParameters(requestUri.Split('/').Last());
    }

    public static IEnumerable<object[]> GetEndpoints()
    {
        yield return new object[] { "/.well-known/host-meta" };
        yield return new object[] { "/.well-known/nodeinfo" };
        yield return new object[] { "/nodeinfo/2.0.json" };
        yield return new object[] { "/.well-known/webfinger?resource=acct:test@localhost" };
    }
}
