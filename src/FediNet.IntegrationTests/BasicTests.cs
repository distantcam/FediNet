using AutoCtor;
using FluentAssert;

namespace FediNet.IntegrationTests;

[AutoConstruct]
[UsesVerify]
public partial class BasicTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    [Theory]
    [InlineData("/.well-known/nodeinfo")]
    [InlineData("/nodeinfo/2.0.json")]
    [InlineData("/.well-known/webfinger?resource=acct:test@host")]
    [InlineData("/.well-known/host-meta", Skip = "Verify broken for content type")]
    public async Task VerifyResponses(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        await Verify(response).UseParameters(url.Replace('/', '_'));
    }

    [Theory]
    [InlineData("")]
    [InlineData("application/json")]
    public async Task UserPageRejectsMissingAcceptHeader(string accept)
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", accept);

        // Act
        var response = await client.GetAsync("/users/test");

        // Assert
        response.StatusCode.ShouldBeEqualTo(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UserPageAcceptsCorrectAcceptHeader()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/ld+json");

        // Act
        var response = await client.GetAsync("/users/test");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task VerifyHostMeta()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/.well-known/host-meta");

        // Assert
        await Verify(await response.Content.ReadAsStringAsync());
    }
}
