namespace GalaFamilyLibrary.Infrastructure.Options;

/// <summary>
/// ORM连接配置
/// </summary>
public sealed class SqlSugarOptions : OptionsBase
{
    public const string SectionName = "SqlSugar";

    public const string DatabaseHost= "DATABASE_HOST";

    public const string DatabaseUser = "DATABASE_USER";

    public const string DatabasePassword = "DATABASE_PASSWORD";

    public const string DatabasePort = "DATABASE_PORT";

    public const string DatabaseName = "DATABASE_DATABASE";

    public SnowFlakeOptions? SnowFlake { get; set; }

    public string? Server { get; set; }

    public int Port { get; set; }

    public string? Database { get; set; }

    public string? User { get; set; }

    public string? Password { get; set; }
}

public class SnowFlakeOptions : OptionsBase
{
    public const string SnowFakeWorkId = "SNOWFLAKES_WORKER_ID";

    public int WorkerId { get; set; }
}