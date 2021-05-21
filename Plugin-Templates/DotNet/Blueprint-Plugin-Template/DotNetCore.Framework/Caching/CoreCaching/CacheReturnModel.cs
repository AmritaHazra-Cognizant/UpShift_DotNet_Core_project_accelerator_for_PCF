using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.Caching.CoreCaching
{
   public class CacheReturnModel
    {
        public object CacheEntry { get; set; }
        public IDictionary<string, object> CacheContext { get; set; }
    }
}
