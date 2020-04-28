using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetGameShared.Util;
using Optional;

namespace Tests
{
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public void Cache_BelowMaxSize_AddsAndGets()
        {
            var cache = new Cache<uint, string>(3);
            cache.Add(1, "one");
            cache.Add(2, "two");
            cache.Add(3, "three");
            Assert.AreEqual(cache.Get(1), "one".Some());
            Assert.AreEqual(cache.Get(2), "two".Some());
            Assert.AreEqual(cache.Get(3), "three".Some());
            Assert.AreEqual(cache.Count, 3);
        }

        [TestMethod]
        public void Cache_AboveMaxSize_AddsAndGets()
        {
            var cache = new Cache<uint, string>(3);
            cache.Add(1, "one");
            cache.Add(2, "two");
            cache.Add(3, "three");

            cache.Add(4, "four");
            Assert.AreEqual(cache.Get(1), Option.None<string>());
            Assert.AreEqual(cache.Get(2), "two".Some());
            Assert.AreEqual(cache.Get(3), "three".Some());
            Assert.AreEqual(cache.Get(4), "four".Some());
            Assert.AreEqual(cache.Count, 3);

            cache.Add(5, "five");
            Assert.AreEqual(cache.Get(1), Option.None<string>());
            Assert.AreEqual(cache.Get(2), Option.None<string>());
            Assert.AreEqual(cache.Get(3), "three".Some());
            Assert.AreEqual(cache.Get(4), "four".Some());
            Assert.AreEqual(cache.Get(5), "five".Some());
            Assert.AreEqual(cache.Count, 3);

            cache.Add(6, "six");
            Assert.AreEqual(cache.Get(1), Option.None<string>());
            Assert.AreEqual(cache.Get(2), Option.None<string>());
            Assert.AreEqual(cache.Get(3), Option.None<string>());
            Assert.AreEqual(cache.Get(4), "four".Some());
            Assert.AreEqual(cache.Get(5), "five".Some());
            Assert.AreEqual(cache.Get(6), "six".Some());
            Assert.AreEqual(cache.Count, 3);
        }
    }
}
