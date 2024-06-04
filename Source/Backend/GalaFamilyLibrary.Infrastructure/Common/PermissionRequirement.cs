using Microsoft.IdentityModel.Tokens;

namespace GalaFamilyLibrary.Infrastructure.Common
{
    public class PermissionRequirement
    {
        public string ClaimType { get; }

        public string Issuer { get; }

        public string Audience { get; }

        public TimeSpan Expiration { get; }

        public SigningCredentials SigningCredentials { get; }

        public PermissionRequirement(string claimType, string issuer, string audience, TimeSpan expiration, SigningCredentials credentials)
        {
            Expiration = expiration;
            ClaimType = claimType;
            Issuer = issuer;
            Audience = audience;
            SigningCredentials = credentials;
        }
    }
}
