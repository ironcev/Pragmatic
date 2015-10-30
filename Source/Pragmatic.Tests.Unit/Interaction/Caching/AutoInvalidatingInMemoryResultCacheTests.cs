using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Pragmatic.Interaction;
using Pragmatic.Interaction.Caching;

namespace Pragmatic.Tests.Unit.Interaction.Caching
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class AutoInvalidatingInMemoryResultCacheTests
    {
        private TestQuery _testQuery;
        private object _result;

        [SetUp]
        public void Setup()
        {
            _testQuery = new TestQuery();
            _result = new object();
        }

        [Test]
        public void TryGetCachedResultFor_EmptyCache_ReturnsDefault()
        {
            var cache = new TestQueryResultCache(TimeSpan.FromMilliseconds(10));

            object result;
            Assert.That(cache.TryGetCachedResultFor(_testQuery, out result), Is.False);
            Assert.That(result, Is.EqualTo(default(object)));
        }

        [Test]
        public void TryGetCachedResultFor_ObjectInCache_ReturnsObject()
        {
            var cache = new TestQueryResultCache(TimeSpan.FromMilliseconds(10));
            cache.CacheResultFor(_testQuery, _result);

            object result;
            Assert.That(cache.TryGetCachedResultFor(_testQuery, out result), Is.True);
            Assert.That(result, Is.SameAs(_result));
        }

        [Test]
        public void TryGetCachedResultFor_ObjectNotInCache_ReturnsDefault()
        {
            var cache = new TestQueryResultCache(TimeSpan.FromMilliseconds(10));
            cache.CacheResultFor(_testQuery, _result);

            object result;
            Assert.That(cache.TryGetCachedResultFor(new TestQuery(), out result), Is.False);
            Assert.That(result, Is.EqualTo(default(object)));
        }

        [Test]
        public void TryGetCachedResultFor_ObjectExpired_ReturnsDefault()
        {
            var cache = new TestQueryResultCache(TimeSpan.FromMilliseconds(10));
            cache.CacheResultFor(_testQuery, _result);

            Task.Delay(12).Wait();

            object result;
            Assert.That(cache.TryGetCachedResultFor(_testQuery, out result), Is.False);
            Assert.That(result, Is.EqualTo(default(object)));
        }

        [Test]
        public void CacheResultFor_ReplacesAlreadyCachedResult()
        {
            var cache = new TestQueryResultCache(TimeSpan.FromMilliseconds(10));
            cache.CacheResultFor(_testQuery, _result);

            var newResult = new object();
            cache.CacheResultFor(_testQuery, newResult);

            object result;
            Assert.That(cache.TryGetCachedResultFor(_testQuery, out result), Is.True);
            Assert.That(result, Is.SameAs(newResult));
        }


        [Test]
        public void Cached_results_expire_after_time_span()
        {
            // It takes roughly 10 milliseconds to cache 100 query results.
            // So we will put 50 milliseconds here.
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(50);
            var cache = new TestQueryResultCache(timeSpan);

            var queriesAndResultsToCache = Enumerable.Range(1, 100).Select(x => new { Query = new TestQuery(), Result = new object() }).ToArray();

            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (var toCache in queriesAndResultsToCache)
                cache.CacheResultFor(toCache.Query, toCache.Result);
            stopwatch.Stop();

            var lastResultCachedOn = DateTimeOffset.UtcNow;

            Console.WriteLine("{0} query results cached in {1} milliseconds.", queriesAndResultsToCache.Length, stopwatch.ElapsedMilliseconds);
            
            foreach (var toCache in queriesAndResultsToCache)
            {
                object result;
                Assert.That(cache.TryGetCachedResultFor(toCache.Query, out result), Is.True);
                Assert.That(result, Is.SameAs(toCache.Result));
            }

            // Let's wait for a few milliseconds more then the time after the
            // last item should expire.
            var timeSpanToWait = timeSpan.Add(lastResultCachedOn - DateTimeOffset.UtcNow).Add(TimeSpan.FromMilliseconds(10));
            Task.Delay(timeSpanToWait).Wait();

            foreach (var toCache in queriesAndResultsToCache)
            {
                object result;
                Assert.That(cache.TryGetCachedResultFor(toCache.Query, out result), Is.False);
                Assert.That(result, Is.EqualTo(default(object)));
            }
        }

        [Test]
        public void InvalidateCacheForAllQueries_InvalidatesCacheForAllQueries()
        {
            // It takes roughly 10 milliseconds to cache 100 query results.
            // So we will put 50 milliseconds here.
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(50);
            var cache = new TestQueryResultCache(timeSpan);

            var queriesAndResultsToCache = Enumerable.Range(1, 100).Select(x => new { Query = new TestQuery(), Result = new object() }).ToArray();

            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (var toCache in queriesAndResultsToCache)
                cache.CacheResultFor(toCache.Query, toCache.Result);
            stopwatch.Stop();

            Console.WriteLine("{0} query results cached in {1} milliseconds.", queriesAndResultsToCache.Length, stopwatch.ElapsedMilliseconds);

            foreach (var toCache in queriesAndResultsToCache)
            {
                object result;
                Assert.That(cache.TryGetCachedResultFor(toCache.Query, out result), Is.True);
                Assert.That(result, Is.SameAs(toCache.Result));
            }

            cache.InvalidateCacheForAllQueries();

            foreach (var toCache in queriesAndResultsToCache)
            {
                object result;
                Assert.That(cache.TryGetCachedResultFor(toCache.Query, out result), Is.False);
                Assert.That(result, Is.EqualTo(default(object)));
            }
        }

        [Test]
        public void InvalidateCacheFor_InvalidatesCacheOnlyForSelectedQueries()
        {
            // It takes roughly 10 milliseconds to cache 100 query results.
            // So we will put 50 milliseconds here.
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(50);
            var cache = new TestQueryResultCache(timeSpan);

            var queriesAndResultsToCache = Enumerable.Range(1, 100).Select(x => new { Query = new TestQuery(), Result = new object() }).ToArray();

            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (var toCache in queriesAndResultsToCache)
                cache.CacheResultFor(toCache.Query, toCache.Result);
            stopwatch.Stop();

            Console.WriteLine("{0} query results cached in {1} milliseconds.", queriesAndResultsToCache.Length, stopwatch.ElapsedMilliseconds);

            foreach (var toCache in queriesAndResultsToCache)
            {
                object result;
                Assert.That(cache.TryGetCachedResultFor(toCache.Query, out result), Is.True);
                Assert.That(result, Is.SameAs(toCache.Result));
            }

            var queriesToInvalidate = queriesAndResultsToCache
                .Take(queriesAndResultsToCache.Length/2)
                .Select(item => item.Query)
                .ToArray();

            cache.InvalidateCacheFor(queriesToInvalidate.Contains);

            foreach (var query in queriesToInvalidate)
            {
                object result;
                Assert.That(cache.TryGetCachedResultFor(query, out result), Is.False);
                Assert.That(result, Is.EqualTo(default(object)));
            }

            foreach (var toCache in queriesAndResultsToCache.Where(item => !queriesToInvalidate.Contains(item.Query)))
            {
                object result;
                Assert.That(cache.TryGetCachedResultFor(toCache.Query, out result), Is.True);
                Assert.That(result, Is.SameAs(toCache.Result));
            }
        }

        public class TestQuery : IEquatableQuery<TestQuery, object>
        {
            public bool IsEquivalentTo(TestQuery query)
            {
                return ReferenceEquals(this, query);
            }
        }

        public class TestQueryResultCache : AutoInvalidatingInMemoryResultCache<TestQuery, object>
        {
            public TestQueryResultCache(TimeSpan timeSpan) : base(timeSpan)
            {
            }
        }
    }
    // ReSharper restore InconsistentNaming
}
