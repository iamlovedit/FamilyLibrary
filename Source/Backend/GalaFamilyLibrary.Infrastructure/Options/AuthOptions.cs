namespace GalaFamilyLibrary.Infrastructure.Options;

/// <summary>
/// Jwt配置
/// </summary>
public sealed class AuthOptions : OptionsBase
{
    public const string SectionName = "Auth";

    public const string AuthIssuer = "AUTH_ISSUER";

    public const string AuthAudience = "AUTH_AUDIENCE";

    public const string AuthSecurityKey = "AUTH_KEY";

    public const string AuthDuration = "AUTH_DURATION";

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string SecurityKey { get; set; }

    public int Duration { get; set; }

    public string? Policy { get; set; }

    public string[]? Roles { get; set; }
}