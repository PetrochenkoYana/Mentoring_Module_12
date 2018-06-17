using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CacheLibrary;
using NorthwindLibrary;

namespace EntitiesCache
{
    public class EntitiesManager<T> where T : class
    {
        private ICache<IEnumerable<T>> cache;

        public EntitiesManager(ICache<IEnumerable<T>> cache)
        {
            this.cache = cache;
        }

        public IEnumerable<T> GetEntities()
        {
            Console.WriteLine("Get Entities");

            var cacheKey = Thread.CurrentPrincipal.Identity.Name + " " + typeof(T);
            var entities = cache.Get(cacheKey);

            if (entities == null)
            {
                Console.WriteLine("From DB");

                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    entities = dbContext.Set<T>().ToList();
                    cache.Set(cacheKey, entities);
                }
            }

            return entities;
        }
    }
}
