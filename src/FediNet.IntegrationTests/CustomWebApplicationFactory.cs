using Microsoft.AspNetCore.Mvc.Testing;

namespace FediNet.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<IApiMarker>
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
