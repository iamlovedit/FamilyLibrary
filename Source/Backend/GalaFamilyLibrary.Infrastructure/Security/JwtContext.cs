namespace GalaFamilyLibrary.Infrastructure.Security;

/// <summary>
/// jwt配置上下文
/// </summary>
/// <param name="issuer"></param>
/// <param name="audience"></param>
/// <param name="duration"></param>
/// <param name="credentials"></param>
public class JwtContext(
    string issuer,
    string audience,
    int duration,
    SigningCredentials credentials)
{
    public string Issuer { get; } = issuer;

    public string Audience { get; } = audience;

    public int Duration { get; } = duration;

    public SigningCredentials SigningCredentials { get; } = credentials;
}