using System.Runtime.Caching;
using CachePolicyService;

namespace CacheLibrary
{
    public class MemoryCache<T> : ICache<T>
    {
        ObjectCache cache = MemoryCache.Default;
        string prefix = "Cache_by_";

        public ICachePolicyService CachePolicy { get; set; }

        public MemoryCache(ICachePolicyService cachePolicyService)
        {
            CachePolicy = cachePolicyService;
        }

        public T Get(string key)
        {
            var value = cache.Get(prefix + key);
            if (value == null)
            {
                return default(T);
            }

            return (T)value;
        }

        public void Set(string key, T entities)
        {
            cache.Set((prefix + key), entities, CachePolicy.GetCachePolicy());
        }

    }
}
