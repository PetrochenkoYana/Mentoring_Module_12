using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Reflection;
using CacheLibrary;
using EntitiesCache;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using CachePolicyService;
using System.Configuration;

namespace EntitiesCache_Tests
{
    [TestClass]
    public class EntitiesCacheTests
    {
        private ICachePolicyService cachePolicyService;

        [TestInitialize]
        public void GetCachePolicy()
        {
            cachePolicyService = new CachePolicyService.CachePolicyService();
        }

        [TestMethod]
        public void CategoriesUsingMemoryCache()
        {
            //Arrange
            var entityManager = new EntitiesManager<Category>(new MemoryCache<IEnumerable<Category>>(cachePolicyService));

            FieldInfo cacheField = entityManager.GetType().GetField("cache", BindingFlags.NonPublic | BindingFlags.Instance);
            object cacheFieldValue = cacheField.GetValue(entityManager);
            MethodInfo method = cacheFieldValue.GetType().GetMethod("Get", new Type[] { typeof(string) });

            //Act
            var entities = entityManager.GetEntities();
            IEnumerable<Category> cacheValue = (IEnumerable<Category>)method.Invoke(cacheFieldValue,
                new object[] { Thread.CurrentPrincipal.Identity.Name + " " + typeof(Category) });

            //Assert
            Assert.IsTrue(entities != null);
            Assert.IsTrue(cacheValue != null);
            Assert.IsTrue(cacheValue.Any());
        }

        [TestMethod]
        public void CategoriesUsingRedisCache()
        {
            //Arrange
            var entityManager = new EntitiesManager<Category>(new RedisCache<IEnumerable<Category>>("localhost", cachePolicyService));

            FieldInfo cacheField = entityManager.GetType().GetField("cache", BindingFlags.NonPublic | BindingFlags.Instance);
            object cacheFieldValue = cacheField.GetValue(entityManager);
            MethodInfo method = cacheFieldValue.GetType().GetMethod("Get", new Type[] { typeof(string) });

            //Act
            var entities = entityManager.GetEntities();
            IEnumerable<Category> cacheValue = (IEnumerable<Category>)method.Invoke(cacheFieldValue,
                new object[] { Thread.CurrentPrincipal.Identity.Name + " " + typeof(Category) });

            //Assert
            Assert.IsTrue(entities != null);
            Assert.IsTrue(cacheValue != null);
            Assert.IsTrue(cacheValue.Any());
        }

        [TestMethod]
        public void CustomersUsingMemoryCache()
        {
            //Arrange
            var entityManager = new EntitiesManager<Customer>((new MemoryCache<IEnumerable<Customer>>(cachePolicyService)));

            FieldInfo cacheField = entityManager.GetType().GetField("cache", BindingFlags.NonPublic | BindingFlags.Instance);
            object cacheFieldValue = cacheField.GetValue(entityManager);
            MethodInfo method = cacheFieldValue.GetType().GetMethod("Get", new Type[] { typeof(string) });

            //Act
            var entities = entityManager.GetEntities();
            IEnumerable<Customer> cacheValue = (IEnumerable<Customer>)method.Invoke(cacheFieldValue,
                new object[] { Thread.CurrentPrincipal.Identity.Name + " " + typeof(Customer) });

            //Assert
            Assert.IsTrue(entities != null);
            Assert.IsTrue(cacheValue != null);
            Assert.IsTrue(cacheValue.Any());
        }

        [TestMethod]
        public void CustomersUsingRedisCache()
        {
            //Arrange
            var entityManager = new EntitiesManager<Customer>(new RedisCache<IEnumerable<Customer>>("localhost", cachePolicyService));

            FieldInfo cacheField = entityManager.GetType().GetField("cache", BindingFlags.NonPublic | BindingFlags.Instance);
            object cacheFieldValue = cacheField.GetValue(entityManager);
            MethodInfo method = cacheFieldValue.GetType().GetMethod("Get", new Type[] { typeof(string) });

            //Act
            var entities = entityManager.GetEntities();
            IEnumerable<Customer> cacheValue = (IEnumerable<Customer>)method.Invoke(cacheFieldValue,
                new object[] { Thread.CurrentPrincipal.Identity.Name + " " + typeof(Customer) });

            //Assert
            Assert.IsTrue(entities != null);
            Assert.IsTrue(cacheValue != null);
            Assert.IsTrue(cacheValue.Any());
        }

        //works for  <!--<add key="PolicyType" value="ExpirationTime" />-->
        [TestMethod]
        public void ExcpirationTimeTestInvalidation()
        {
            //Arrange
            var entityManager = new EntitiesManager<Category>(new MemoryCache<IEnumerable<Category>>(cachePolicyService));

            FieldInfo cacheField = entityManager.GetType().GetField("cache", BindingFlags.NonPublic | BindingFlags.Instance);
            object cacheFieldValue = cacheField.GetValue(entityManager);
            MethodInfo method = cacheFieldValue.GetType().GetMethod("Get", new Type[] { typeof(string) });
            double interval = Convert.ToDouble(ConfigurationManager.AppSettings["TimeIntervalMilliseconds"]) + 1;

            //Act
            var entities = entityManager.GetEntities();
            System.Threading.Thread.Sleep(Convert.ToInt32(interval));

            IEnumerable<Category> cacheValue = (IEnumerable<Category>)method.Invoke(cacheFieldValue,
                new object[] { Thread.CurrentPrincipal.Identity.Name + " " + typeof(Category) });

            //Assert
            Assert.IsTrue(entities != null);
            Assert.IsTrue(cacheValue == null);
        }

        [TestMethod]
        public void ExcpirationTimeTestValidation()
        {
            //Arrange
            var entityManager = new EntitiesManager<Category>(new MemoryCache<IEnumerable<Category>>(cachePolicyService));

            FieldInfo cacheField = entityManager.GetType().GetField("cache", BindingFlags.NonPublic | BindingFlags.Instance);
            object cacheFieldValue = cacheField.GetValue(entityManager);
            MethodInfo method = cacheFieldValue.GetType().GetMethod("Get", new Type[] { typeof(string) });
            double interval = Convert.ToDouble(ConfigurationManager.AppSettings["TimeIntervalMilliseconds"]) - 200;

            //Act
            var entities = entityManager.GetEntities();
            Thread.Sleep(Convert.ToInt32(interval));

            IEnumerable<Category> cacheValue = (IEnumerable<Category>)method.Invoke(cacheFieldValue,
                new object[] { Thread.CurrentPrincipal.Identity.Name + " " + typeof(Category) });

            //Assert
            Assert.IsTrue(entities != null);
            Assert.IsTrue(cacheValue != null);
            Assert.IsTrue(cacheValue.Any());
        }

        [TestMethod]
        public void CacheItemPolicy()
        {
            //Arrange
            var entityManager = new EntitiesManager<Category>(new MemoryCache<IEnumerable<Category>>(cachePolicyService));

            FieldInfo cacheField = entityManager.GetType().GetField("cache", BindingFlags.NonPublic | BindingFlags.Instance);
            object cacheFieldValue = cacheField.GetValue(entityManager);
            MethodInfo method = cacheFieldValue.GetType().GetMethod("Get", new Type[] { typeof(string) });

            //Act
            var entities = entityManager.GetEntities();
            using (var dbContext = new Northwind())
            {
                dbContext.Configuration.LazyLoadingEnabled = false;
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Categories.Add(new Category { CategoryName = "Milk products" });
                dbContext.SaveChanges();
            }
            Thread.Sleep(3000);
            IEnumerable<Category> cacheValue = (IEnumerable<Category>)method.Invoke(cacheFieldValue,
                new object[] { Thread.CurrentPrincipal.Identity.Name + " " + typeof(Category) });

            //Assert
            Assert.IsTrue(entities != null);
            Assert.IsTrue(cacheValue == null);
        }
    }
}
