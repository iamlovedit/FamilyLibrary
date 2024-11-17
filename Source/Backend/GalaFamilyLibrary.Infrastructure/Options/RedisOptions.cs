namespace GalaFamilyLibrary.Infrastructure.Options;

/// <summary>
/// redis连接配置
/// </summary>
public sealed class RedisOptions : OptionsBase
{
    public const string SectionName = "Redis";

    public const string RedisServiceName = "REDIS_SERVICE";

    public const string RedisPassword = "REDIS_PASSWORD";

    public const string RedisHost = "REDIS_HOST";

    public string ServiceName { get; set; }

    public string Host { get; set; }

    public string Password { get; set; }
}