using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Framework.Caching.CoreCaching
{
    public interface ICacheConfigFactory
    {
        Dictionary<string, CacheConfig> Config { get; }
    }
}
