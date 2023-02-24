using System.Collections.Concurrent;

public class RedisCache
{
    private ConcurrentDictionary<string, string> map = new ConcurrentDictionary<string, string>();

    public void Set(string key, string value)
    {
        this.map.TryAdd(key.Trim(), value.Trim());
    }

    public string Get(string key)
    {
        if (this.map.ContainsKey(key))
        {
            return this.map[key];
        }
        else 
        {
            Console.WriteLine("Key not found");
            return "";
        }
    }
}
