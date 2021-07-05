using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.Caching.CoreCaching
{
   public interface ICacheable
    {
        DateTime TTL { get; set; }
        DateTime CachedUtc { get; set; }

    }
}
