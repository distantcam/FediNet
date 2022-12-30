using AutoCtor;

namespace FediNet.IntegrationTests.Features;

[AutoConstruct]
[UsesVerify]
[Collection("Test Collection")]
public partial class OutboxTests
{
    private readonly CustomApiFactory _factory;

    [Fact]
    public async Task VerifyOutboxPage()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/users/test/outbox");

        // Assert
        await Verify(response);
    }
}
