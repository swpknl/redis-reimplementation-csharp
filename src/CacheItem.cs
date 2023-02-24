public class CacheItem
{
    private readonly string value;
    private readonly DateTime creationTime;
    private readonly int? ttl;

    public CacheItem(string value, DateTime creationTime, int? ttl)
    {
        this.value = value;
        this.creationTime = creationTime;
        this.ttl = ttl;
    }

    public string Value => value;

    public DateTime CreationTime => creationTime;

    public int? Ttl => ttl;
}