using System.Collections.Concurrent;

public class RedisCache
{
    private ConcurrentDictionary<string, CacheItem> map =
        new ConcurrentDictionary<string, CacheItem>();

    public void Set(string key, string value, int? ttl)
    {
        var cacheItem = new CacheItem(value, DateTime.Now, ttl);
        if (this.map.ContainsKey(key))
        {
            this.map[key] = cacheItem;
        }
        else
        {
            this.map.TryAdd(key, cacheItem);
        }
    }

    public string Get(string key)
    {
        string result = null;
        if (this.map.ContainsKey(key))
        {
            var value = this.map[key];
            if (value.Ttl.HasValue)
            {
                var diff = (DateTime.Now - value.CreationTime);
                if (diff.Milliseconds <= value.Ttl.Value)
                {
                    result = $"+{this.map[key].Value}\r\n";
                }
                else
                {
                    CacheItem item;
                    this.map.Remove(key, out item);
                    result = Constants.NullString;
                }
            }
            else
            {
                result = $"+{this.map[key].Value}\r\n";
            }
        }
        else
        {
            result = Constants.NullString;
        }

        return result;
    }
}
