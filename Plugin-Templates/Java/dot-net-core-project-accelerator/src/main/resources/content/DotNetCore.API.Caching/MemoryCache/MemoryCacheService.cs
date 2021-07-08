using DotNetCore.Framework.Caching.CoreCaching;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.API.Caching.MemoryCache
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private const int MINUTES_TO_MILISECONDS_MULTIPLIER = 60000;
        private IMemoryCache _memoryCache;
        private readonly ICacheConfigFactory _cacheConfigFactory;
        public MemoryCacheService(IMemoryCache memoryCache, ICacheConfigFactory cacheConfigFactory )
        {
            _memoryCache = memoryCache;
            _cacheConfigFactory = cacheConfigFactory;
        }

        public void Clear(CacheConfig config)
        {
            this._memoryCache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
        }

        public ICacheable Get(string cacheKey)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
                throw new ArgumentNullException("cacheKey");
            ICacheable rtrn = null;
            this._memoryCache.TryGetValue(cacheKey, out var result);

            if (result != null)
            {
                rtrn = result as ICacheable;
            }
            return rtrn;
        }

        public object GetCollection(string cacheKey, Type elementType)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
                throw new ArgumentNullException("cacheKey");
            Type customList = typeof(List<>).MakeGenericType(elementType);
            IList objectList = null;
            object cacheEntry = Get(cacheKey);

            if (cacheEntry != null)
            {
                objectList = (IList)Activator.CreateInstance(customList);
                foreach (var item in ((IEnumerable)cacheEntry))
                {
                    objectList.Add(item);
                }
            }
            return objectList;
        }
        public List<string> GetAllKeys(CacheConfig config)
        {
            throw new NotImplementedException();
        }

        public void Put(string cacheKey, ICacheable dataToCache)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
                throw new ArgumentNullException("cacheKey");
            var cacheKeyInfo = this._cacheConfigFactory.Config[cacheKey];
            if (cacheKeyInfo == null)
                throw new ArgumentNullException("Key not configuired");

            dataToCache.TTL = DateTime.Now.AddMilliseconds(
                cacheKeyInfo.TimeToLiveMinutes * MINUTES_TO_MILISECONDS_MULTIPLIER);
            dataToCache.CachedUtc = DateTime.Now;
            this._memoryCache.Set(cacheKey, dataToCache, dataToCache.TTL.TimeOfDay);

        }

        public void Remove(string cacheKey)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
                throw new ArgumentNullException("cacheKey");
            this._memoryCache.Remove(cacheKey);

        }
    }
}
