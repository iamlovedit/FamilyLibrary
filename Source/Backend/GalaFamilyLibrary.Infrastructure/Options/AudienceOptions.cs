namespace GalaFamilyLibrary.Infrastructure.Options;

/// <summary>
/// Jwt配置
/// </summary>
public sealed class AudienceOptions : OptionsBase
{
    public const string SectionName = "Audience";

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Secret { get; set; }

    public int Duration { get; set; }

    public string? Policy { get; set; }

    public string[]? Roles { get; set; }
}