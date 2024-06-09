namespace GalaFamilyLibrary.Infrastructure.Security
{
    public class TokenInfo(double expiredIn, string tokenType)
    {
        public string? Token { get; }

        public double? ExpiredIn { get; } = expiredIn;

        public string? TokenType { get; } = tokenType;

        public string? RefreshToken { get; set; }

        public double? RefreshExpiredIn { get; set; }

        public TokenInfo(string token, double expiredIn, string tokenType) : this(expiredIn, tokenType)
        {
            Token = token;
        }
    }
}
