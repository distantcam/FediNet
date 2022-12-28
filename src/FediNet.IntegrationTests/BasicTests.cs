using AutoCtor;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FediNet.IntegrationTests;

[AutoConstruct]
[UsesVerify]
public partial class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    [Theory]
    [InlineData("/.well-known/nodeinfo")]
    [InlineData("/nodeinfo/2.0.json")]
    public async Task VerifyResponses(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        await Verify(response).UseParameters(url.Replace('/', '_'));
    }
}
