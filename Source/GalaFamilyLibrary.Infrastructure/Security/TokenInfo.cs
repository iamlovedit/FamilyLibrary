namespace GalaFamilyLibrary.Infrastructure.Security
{
    public class TokenInfo
    {
        public string? Token { get; }

        public double? ExpiredIn { get; }

        public string? TokenType { get; }

        public string? RefreshToken { get; set; }

        public double? RefreshExpiredIn { get; set; }

        public TokenInfo(double expiredIn, string tokenType)
        {
            ExpiredIn = expiredIn;
            TokenType = tokenType;
        }
        public TokenInfo(string token, double expiredIn, string tokenType) : this(expiredIn, tokenType)
        {
            Token = token;
        }
    }
}
