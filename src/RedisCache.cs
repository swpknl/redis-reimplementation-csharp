public class RedisCache
{
    private Dictionary<string, string> map = new Dictionary<string, string>();

    public void Set(string key, string value) 
    {
        this.map.Add(key, value);
    }

    public string Get(string key)
    {
        return this.map[key];
    }
}