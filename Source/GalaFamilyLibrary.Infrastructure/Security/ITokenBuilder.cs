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

    public class TokenBuilder : ITokenBuilder
    {
        private readonly IAESEncryptionService _aesEncryptionService;
        private readonly PermissionRequirement _permissionRequirement;

        public TokenBuilder(IAESEncryptionService aesEncryptionService, PermissionRequirement permissionRequirement)
        {
            _aesEncryptionService = aesEncryptionService;
            _permissionRequirement = permissionRequirement;
        }

        public TokenInfo GenerateTokenInfo(IReadOnlyCollection<Claim> claims)
        {
            var jwtToken = new JwtSecurityToken(
                   issuer: _permissionRequirement.Issuer,
                   audience: _permissionRequirement.Audience,
                   claims: claims,
                   notBefore: DateTime.Now,
                   expires: DateTime.Now.Add(_permissionRequirement.Expiration),
                   signingCredentials: _permissionRequirement.SigningCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            token = _aesEncryptionService.Encrypt(token);
            return new TokenInfo(token, _permissionRequirement.Expiration.TotalSeconds, JwtBearerDefaults.AuthenticationScheme);
        }
    }
}
