using Injectio.Attributes;
using Microsoft.Extensions.Caching.Memory;

namespace FediNet.Caching;

[RegisterSingleton<PublicKeyCache>]
public class PublicKeyCache : MemoryCache
{
    public PublicKeyCache(ILoggerFactory loggerFactory) :
        base(new MemoryCacheOptions(), loggerFactory)
    {
    }
}
