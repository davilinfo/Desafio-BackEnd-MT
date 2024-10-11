using Application.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConfiguration _configuration;        
        private readonly IDistributedCache _cache;

        public RedisCacheService(IConfiguration configuration, IDistributedCache cache)
        {
            _configuration = configuration;            
            _cache = cache;
        }

        public void SetValue(string key, string value)
        {
            
            _cache.SetString(key, value, new DistributedCacheEntryOptions { AbsoluteExpiration = DateTime.Now.AddMinutes(30) });
        }

        public string GetValue(string key)
        {
#pragma warning disable CS8603
            return _cache.GetString(key);
#pragma warning restore CS8603
        }

        public void InvalidateCacheEntry(string key)
        {
            _cache.Remove(key);
        }
    }

}
