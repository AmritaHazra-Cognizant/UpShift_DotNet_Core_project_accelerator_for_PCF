using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.Caching.CoreCaching
{
    public class Cacheable<T> : ICacheable
    {
        /// <summary>
        /// TTL is time to live as expressed om the expiration 
        /// date and time after which
        /// the object is no longer valid
        /// </summary>
        public DateTime TTL { get; set; }

        /// <summary>
        /// Cache DateTime
        /// </summary>
        public DateTime CachedUtc { get; set; }


        /// <summary>
        /// Generic Model that you want to cache
        /// </summary>
        public T Model { get; set; }
    }
}
