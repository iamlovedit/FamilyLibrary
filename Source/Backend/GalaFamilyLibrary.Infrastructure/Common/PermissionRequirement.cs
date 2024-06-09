using Microsoft.IdentityModel.Tokens;

namespace GalaFamilyLibrary.Infrastructure.Common
{
    public class PermissionRequirement(
        string claimType,
        string issuer,
        string audience,
        TimeSpan expiration,
        SigningCredentials credentials)
    {
        public string ClaimType { get; } = claimType;

        public string Issuer { get; } = issuer;

        public string Audience { get; } = audience;

        public TimeSpan Expiration { get; } = expiration;

        public SigningCredentials SigningCredentials { get; } = credentials;
    }
}
