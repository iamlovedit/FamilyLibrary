namespace GalaFamilyLibrary.Infrastructure.Security;

/// <summary>
/// 返回客户端的jwt信息
/// </summary>
/// <param name="token"></param>
/// <param name="duration"></param>
/// <param name="tokenType"></param>
public class JwtTokenInfo(string token, int duration, string tokenType)
{
    public string? Token { get; } = token;

    public int Duration { get; } = duration;

    public string? TokenType { get; } = tokenType;
}