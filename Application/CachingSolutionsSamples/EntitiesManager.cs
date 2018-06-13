using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NorthwindLibrary;

namespace CachingSolutionsSamples
{
    public class EntitiesManager<T> where T : class
    {
        private IEntitiesCache<T> cache;

        public EntitiesManager(IEntitiesCache<T> cache)
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
                    cache.Set(cacheKey, entities, DateTimeOffset.Now.AddMilliseconds(300));
                }
            }

            return entities;
        }
    }
}
