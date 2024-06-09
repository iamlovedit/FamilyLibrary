using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GalaFamilyLibrary.Infrastructure.Security
{
    public interface ITokenBuilder
    {
        TokenInfo GenerateTokenInfo(IReadOnlyCollection<Claim> claims);
    }

    public class TokenBuilder(IAESEncryptionService aesEncryptionService, PermissionRequirement permissionRequirement)
        : ITokenBuilder
    {
        public TokenInfo GenerateTokenInfo(IReadOnlyCollection<Claim> claims)
        {
            var jwtToken = new JwtSecurityToken(
                   issuer: permissionRequirement.Issuer,
                   audience: permissionRequirement.Audience,
                   claims: claims,
                   notBefore: DateTime.Now,
                   expires: DateTime.Now.Add(permissionRequirement.Expiration),
                   signingCredentials: permissionRequirement.SigningCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            token = aesEncryptionService.Encrypt(token);
            return new TokenInfo(token, permissionRequirement.Expiration.TotalSeconds, JwtBearerDefaults.AuthenticationScheme);
        }
    }
}
