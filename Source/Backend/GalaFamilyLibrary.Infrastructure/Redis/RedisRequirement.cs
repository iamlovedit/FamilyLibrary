namespace GalaFamilyLibrary.Infrastructure.Redis;

public class RedisRequirement(TimeSpan cacheTime)
{
    public TimeSpan CacheTime { get; } = cacheTime;
}