using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.Caching.CoreCaching
{
    public class CacheConfig : ICacheConfig
    {
        public const string CacheConfigKey = "CacheConfig";
        public string CacheName { get; set; }
        public string CacheKey { get; set; }
        public int TimeToLiveMinutes { get; set; }
        public bool Enabled { get; set; }
        public CacheConfig()
        {
          
        }
        public CacheConfig(string cacheKey, int timeToLiveMinutes,
            bool enabled, string cacheName = "DotNetCore.API.Caching")
        {
            CacheKey = cacheKey;
            TimeToLiveMinutes = timeToLiveMinutes;
            Enabled = enabled;
            CacheName = cacheName;
        }
    }
}
