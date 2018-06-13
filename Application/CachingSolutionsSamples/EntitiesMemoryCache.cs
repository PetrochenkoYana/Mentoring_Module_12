using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;

namespace CachingSolutionsSamples
{
    internal class EntitiesMemoryCache<T> : IEntitiesCache<T>
    {
        ObjectCache cache = MemoryCache.Default;
        string prefix = "Cache_by_";

        public IEnumerable<T> Get(string key)
        {
            return (IEnumerable<T>)cache.Get(prefix + key);
        }

        public void Set(string key, IEnumerable<T> entities, DateTimeOffset dateTime)
        {
            cache.Set((prefix + key), entities, dateTime);
        }

    }
}
