﻿
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CacheLibrary;
using Fibonacci__Sequence;
using CachePolicyService;

namespace Fibonacci__Sequence_Tests
{
    [TestClass]
    public class Test
    {
        private ICachePolicyService cachePolicyService;

        [TestInitialize]
        public void GetCachePolicy()
        {
            cachePolicyService = new CachePolicyService.CachePolicyService();
        }
        [TestMethod]
        public void MemoryCache()
        {
            var fibonacci = new Fibonacci(new MemoryCache<int>(cachePolicyService));

            var number1 = fibonacci.GetByIndex(5);
            var number2 = fibonacci.GetByIndex(6);
            var number3 = fibonacci.GetByIndex(7);
            Assert.AreEqual(5, number1);
            Assert.AreEqual(8, number2);
            Assert.AreEqual(13, number3);
        }

        [TestMethod]
        public void RedisCache()
        {
            var fibonacci = new Fibonacci(new RedisCache<int>("localhost", cachePolicyService));

            var number1 = fibonacci.GetByIndex(5);
            var number2 = fibonacci.GetByIndex(6);
            var number3 = fibonacci.GetByIndex(7);
            Assert.AreEqual(5, number1);
            Assert.AreEqual(8, number2);
            Assert.AreEqual(13, number3);
        }
    }
}
