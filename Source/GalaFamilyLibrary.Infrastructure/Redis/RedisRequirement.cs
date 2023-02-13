namespace GalaFamilyLibrary.Infrastructure.Redis;

public class RedisRequirement
{
    public TimeSpan CacheTime { get; }

    public RedisRequirement(TimeSpan cacheTime)
    {
        CacheTime = cacheTime;
    }
}