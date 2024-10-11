namespace Application.Interface
{
    public interface IRedisCacheService
    {
        public void SetValue(string key, string value);
        public string GetValue(string key);
        public void InvalidateCacheEntry(string key);
    }
}
