using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.Caching.CoreCaching
{
   public interface ICache
    {
        ICacheable Get(string cacheKey);
        void Put(string cacheKey, ICacheable dataToCache);
        void Remove(string cacheKey);
        List<string> GetAllKeys(CacheConfig config);
        void Clear(CacheConfig config);

        object GetCollection(string cacheKey, Type elementType);
    }
}
