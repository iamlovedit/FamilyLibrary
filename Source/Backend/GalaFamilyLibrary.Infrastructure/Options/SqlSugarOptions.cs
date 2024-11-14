namespace GalaFamilyLibrary.Infrastructure.Options;

/// <summary>
/// ORM连接配置
/// </summary>
public sealed class SqlSugarOptions : OptionsBase
{
    public const string SectionName = "SqlSugar";

    public SnowFlakeOptions? SnowFlake { get; set; }

    public string? Server { get; set; }

    public int Port { get; set; }

    public string? Database { get; set; }

    public string? User { get; set; }

    public string? Password { get; set; }
}

public class SnowFlakeOptions : OptionsBase
{
    public int WorkerId { get; set; }
}