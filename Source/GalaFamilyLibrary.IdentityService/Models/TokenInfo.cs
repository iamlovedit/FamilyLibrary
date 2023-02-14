namespace GalaFamilyLibrary.IdentityService.Models;

public class TokenInfo
{
    public string Token { get; }

    public DateTime ExpiredAt { get; }

    public string TokenType { get; }

    public TokenInfo(string token,DateTime expiredAt,string tokenType)
    {
        Token = token;
        ExpiredAt = expiredAt;
        TokenType = tokenType;
    }
}