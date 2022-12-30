using AutoCtor;

namespace FediNet.IntegrationTests.Features;

[AutoConstruct]
[UsesVerify]
[Collection("Test Collection")]
public partial class UserTests
{
    private readonly CustomApiFactory _factory;

    [Fact]
    public async Task VerifyUserPage()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/ld+json");

        // Act
        var response = await client.GetAsync("/users/test");

        // Assert
        await Verify(response);
    }

    [Fact]
    public async Task VerifyBadUserPage()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/users/test");

        // Assert
        await Verify(response);
    }
}
