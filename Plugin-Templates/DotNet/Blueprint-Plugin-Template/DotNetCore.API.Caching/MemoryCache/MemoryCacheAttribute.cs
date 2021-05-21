using DotNetCore.Framework.Interception.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.API.Caching.MemoryCache
{
    public class MemoryCacheAttribute : MethodInterceptionAttribute
    {
        public string CacheKey { get; set; }

        /// <summary>
        /// Comma seperated  list of params with exact name to construct dynamic cache key
        /// </summary>
        public string CacheKeyParameterSubstitutions { get; set; }

        public bool RefreshCache { get; set; }

        public MemoryCacheAttribute(string cacheKey, string cacheKeyParameterSubstitutions, bool refreshCache = false)
        {
            CacheKey = cacheKey;
            CacheKeyParameterSubstitutions = cacheKeyParameterSubstitutions;
            RefreshCache = refreshCache;
        }
    }
}
