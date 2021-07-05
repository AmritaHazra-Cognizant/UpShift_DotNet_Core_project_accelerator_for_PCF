using DotNetCore.Framework.Caching.CoreCaching;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.API.Caching.MemoryCache
{

    /// <summary>
    /// This Utility will be used for accessing cache from Business layer
    /// without using method interceptions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class MemoryCacheUtility<T>
    {
        public static T GetCacheObject(IMemoryCacheService cacheObj,
            CacheConfig config, string cacheKey)
        {
            T objectFromCache = default(T);
            if (!config.Enabled) return objectFromCache;
            try
            {
                var response = (Cacheable<T>)cacheObj.Get(cacheKey);
                objectFromCache = response != null ? response.Model : default(T);
            }
            catch (Exception ex)
            {
                // LOG
            }
            return objectFromCache;
        }

        public static void StoreCacheObject(T data, IMemoryCacheService cacheObj,
          CacheConfig config, string cacheKey)
        {

            if (!config.Enabled) return;

            var dataToCache = new Cacheable<T>
            {
                TTL = DateTime.Now.AddMinutes(config.TimeToLiveMinutes),
                Model = data
            };
            try
            {
                cacheObj.Remove(cacheKey);
                cacheObj.Put(cacheKey, dataToCache);
            }
            catch (Exception ex)
            {
                // LOG
            }
         
        }
    }


}
