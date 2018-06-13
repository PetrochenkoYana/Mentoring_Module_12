using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Fibonacci__Sequence
{
    public class MemoryCache: ICache
    {
        private ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;
        public int Get(string key)
        {
            return cache.Contains(key) ? (int) cache.Get(key) : default(int);
        }

        public void Set(string key, int number)
        {
            cache.Set(key,number, ObjectCache.InfiniteAbsoluteExpiration);
        }
     
    }
}
