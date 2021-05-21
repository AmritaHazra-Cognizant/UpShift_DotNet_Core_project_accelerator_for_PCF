namespace DotNetCore.Framework.Caching.CoreCaching
{
    public interface ICacheConfig
    {
        string CacheKey { get; set; }
        string CacheName { get; set; }
        bool Enabled { get; set; }
        int TimeToLiveMinutes { get; set; }
    }
}