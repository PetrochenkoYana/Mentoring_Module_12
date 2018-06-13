using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using Moq;
using System.Data.SqlClient;
using System.Net.Sockets;

namespace CachingSolutionsSamples
{
    [TestClass]
    public class EntitiesCacheTests
    {

        [TestMethod]
        public void CategoriesUsingMemoryCache()
        {
            var entityManager = new EntitiesManager<Category>((new EntitiesMemoryCache<Category>()));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entityManager.GetEntities().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void CategoriesUsingRedisCache()
        {
            var entityManager = new EntitiesManager<Category>(new EntitiesRedisCache<Category>("localhost"));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entityManager.GetEntities().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void CustomersUsingMemoryCache()
        {
            var entityManager = new EntitiesManager<Customer>((new EntitiesMemoryCache<Customer>()));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entityManager.GetEntities().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void CustomersUsingRedisCache()
        {
            var entityManager = new EntitiesManager<Customer>(new EntitiesRedisCache<Customer>("localhost"));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entityManager.GetEntities().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void ExcpirationTimeTestInvalidation()
        {
            var entitycache = new Mock<IEntitiesCache<Category>>();
            var entityManager = new EntitiesManager<Category>(entitycache.Object);

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entityManager.GetEntities().Count());
                Thread.Sleep(100);
            }
            entitycache.Verify(
                n => n.Set(It.IsAny<string>(), It.IsAny<IEnumerable<Category>>(), It.IsAny<DateTimeOffset>()), Times.AtLeast(2));

        }

        [TestMethod]
        public void CacheItemPolicy()
        {
            var policy = new CacheItemPolicy();
            policy.ChangeMonitors.Add(new SqlChangeMonitor(new System.Data.SqlClient.SqlDependency(new SqlCommand("Select * from Northwind.Category"))));
            var entitycache = new Mock<IEntitiesCache<Category>>();
            var entityManager = new EntitiesManager<Category>(entitycache.Object);

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entityManager.GetEntities().Count());
                Thread.Sleep(1000);
            }
            entitycache.Verify(
                n => n.Set(It.IsAny<string>(), It.IsAny<IEnumerable<Category>>(), It.IsAny<DateTimeOffset>()), Times.AtLeast(2));

        }
    }
}
