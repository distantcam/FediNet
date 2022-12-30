using AutoCtor;

namespace FediNet.IntegrationTests;

[AutoConstruct]
[UsesVerify]
public partial class UserTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

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
}
