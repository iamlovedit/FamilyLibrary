using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.IdentityModel.Tokens;

namespace GalaFamilyLibrary.Infrastructure.Security
{
    public class GalaTokenValidator(IAESEncryptionService aesEncryptionService) : ISecurityTokenValidator
    {
        public bool CanReadToken(string securityToken)
        {
            return true;
        }

        public bool CanValidateToken { get; } = true;

        public int MaximumTokenSizeInBytes { get; set; }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            var decodeToken = aesEncryptionService.Decrypt(securityToken);
            return new JwtSecurityTokenHandler().ValidateToken(decodeToken, validationParameters, out validatedToken);
        }
    }
}