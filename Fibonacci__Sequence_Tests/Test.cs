
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using Fibonacci__Sequence;

namespace Fibonacci__Sequence_Tests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void MemoryCache()
        {
            var fibonacci = new Fibonacci(new MemoryCache());

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
            var fibonacci = new Fibonacci(new RedisCache("localhost"));

            var number1 = fibonacci.GetByIndex(5);
            var number2 = fibonacci.GetByIndex(6);
            var number3 = fibonacci.GetByIndex(7);
            Assert.AreEqual(5, number1);
            Assert.AreEqual(8, number2);
            Assert.AreEqual(13, number3);
        }
    }
}
