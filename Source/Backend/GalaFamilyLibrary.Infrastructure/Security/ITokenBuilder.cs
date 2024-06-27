using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace GalaFamilyLibrary.Infrastructure.Security
{
    public interface ITokenBuilder
    {
        TokenInfo GenerateTokenInfo(IReadOnlyCollection<Claim> claims);

        string DecryptCipherToken(string cipherToken);

        bool VerifyToken(string token);

        double GetTokenExpirationSeconds();

        long ParseUIdFromToken(string token);
    }

    public class TokenBuilder(
        IAESEncryptionService aesEncryptionService,
        PermissionRequirement permissionRequirement,
        JwtSecurityTokenHandler jwtSecurityTokenHandler,
        IConfiguration configuration)
        : ITokenBuilder
    {
        public string DecryptCipherToken(string cipherToken)
        {
            if (string.IsNullOrEmpty(cipherToken))
            {
                throw new ArgumentException($"{nameof(cipherToken)} is null or empty。", nameof(cipherToken));
            }

            return aesEncryptionService.Decrypt(cipherToken);
        }

        public TokenInfo GenerateTokenInfo(IReadOnlyCollection<Claim> claims)
        {
            var jwtToken = new JwtSecurityToken(
                   issuer: permissionRequirement.Issuer,
                   audience: permissionRequirement.Audience,
                   claims: claims,
                   notBefore: DateTime.Now,
                   expires: DateTime.Now.Add(permissionRequirement.Expiration),
                   signingCredentials: permissionRequirement.SigningCredentials);
            var token = jwtSecurityTokenHandler.WriteToken(jwtToken);
            token = aesEncryptionService.Encrypt(token);
            return new TokenInfo(token, permissionRequirement.Expiration.TotalSeconds, JwtBearerDefaults.AuthenticationScheme);
        }

        public double GetTokenExpirationSeconds()
        {
            return permissionRequirement.Expiration.TotalSeconds;
        }

        public long ParseUIdFromToken(string token)
        {
            if (!jwtSecurityTokenHandler.CanReadToken(token))
            {
                return 0;
            }

            var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(token);
            if (long.TryParse(jwtToken.Id, out var id))
            {
                return id;
            }
            return 0;
        }

        public bool VerifyToken(string token)
        {
            var key = configuration["AUDIENCE_KEY"];
            var keyBuffer = Encoding.ASCII.GetBytes(key!);
            var signingKey = new SymmetricSecurityKey(keyBuffer);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = jwtSecurityTokenHandler.ReadJwtToken(token);
            return jwt.RawSignature == JwtTokenUtilities.CreateEncodedSignature(jwt.RawHeader + "." + jwt.RawPayload, signingCredentials);
        }
    }
}