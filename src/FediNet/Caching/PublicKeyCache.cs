using Microsoft.Extensions.Caching.Memory;

namespace FediNet.Caching;

public class PublicKeyCache : MemoryCache
{
    public PublicKeyCache(ILoggerFactory loggerFactory) :
        base(new MemoryCacheOptions(), loggerFactory)
    {
    }
}
