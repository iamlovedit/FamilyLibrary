namespace GalaFamilyLibrary.IdentityService.Models;

public class TokenInfo
{
    public string Token { get; }

    public double ExpiredIn { get; }

    public string TokenType { get; }

    public TokenInfo(string token,double expiredIn,string tokenType)
    {
        Token = token;
        ExpiredIn = expiredIn;
        TokenType = tokenType;
    }
}