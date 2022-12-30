using Microsoft.AspNetCore.Mvc.Testing;

namespace FediNet.IntegrationTests;

[CollectionDefinition("Test Collection")]
public class SharedTestCollection : ICollectionFixture<CustomApiFactory>
{
}

public class CustomApiFactory : WebApplicationFactory<IApiMarker>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?> {
                { "host", "host" }
            });
        });

        return base.CreateHost(builder);
    }
}
