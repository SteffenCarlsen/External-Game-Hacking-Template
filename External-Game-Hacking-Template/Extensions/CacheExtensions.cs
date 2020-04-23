using System;
using System.Runtime.Caching;

namespace External_Game_Hacking_Template.Extensions
{
    internal class CacheExtensions
    {
        /// <summary>
        ///     Caches data for a period of time in ms and returns either existing data or new data
        /// </summary>
        /// <typeparam name="T">Type of the data to cache.</typeparam>
        /// <param name="method">The method to update the data.</param>
        /// <param name="cacheTimeMs">Expiration of cache in ms.</param>
        /// <param name="cacheIdentifier">Unique identifier of the cache.</param>
        /// <returns>Cached data of generic data type.</returns>
        public static T GetCachedData<T>(Func<T> method, int cacheTimeMs, string cacheIdentifier)
        {
            var cache = MemoryCache.Default;
            if (!(cache[cacheIdentifier] is T dataCache))
            {
                var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMilliseconds(cacheTimeMs) };

                dataCache = method();
                cache.Set(cacheIdentifier, dataCache, policy);
            }

            return dataCache;
        }
    }
}
