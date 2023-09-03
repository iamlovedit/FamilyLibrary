using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.IdentityModel.Tokens;

namespace GalaFamilyLibrary.Infrastructure.Security
{
    public class GalaTokenValidator : ISecurityTokenValidator
    {
        private readonly IAESEncryptionService _aesEncryptionService;

        public GalaTokenValidator(IAESEncryptionService aesEncryptionService)
        {
            _aesEncryptionService = aesEncryptionService;
        }

        public bool CanReadToken(string securityToken)
        {
            return true;
        }

        public bool CanValidateToken { get; }

        public int MaximumTokenSizeInBytes { get; set; }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var decodeToken = _aesEncryptionService.Decrypt(securityToken);
            var principal =
                new JwtSecurityTokenHandler().ValidateToken(decodeToken, validationParameters, out validatedToken);
            return principal;
        }
    }
}