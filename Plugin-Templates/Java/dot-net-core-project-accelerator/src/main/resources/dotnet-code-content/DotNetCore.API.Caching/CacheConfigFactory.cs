using DotNetCore.Framework.Caching.CoreCaching; 
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
namespace DotNetCore.API.Caching
{
    public class CacheConfigFactory : ICacheConfigFactory
    {
        private readonly CacheConfig _cacheConfig;
        public CacheConfigFactory(IOptions<CacheConfig> cacheConfig)
        {
            _cacheConfig = cacheConfig.Value;
        }
        public Dictionary<string, CacheConfig> Config
        {
            get
            {

                var cacheList = new Dictionary<string, CacheConfig>();
                cacheList.Add("RetrieveEmployees", new CacheConfig("RetrieveEmployees", _cacheConfig.TimeToLiveMinutes, _cacheConfig.Enabled, _cacheConfig.CacheName));
                return cacheList;
            }
        }
    }
}
