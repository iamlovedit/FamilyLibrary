using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace GalaFamilyLibrary.Infrastructure.Security
{
    public class GalaTokenHandler(
        IAESEncryptionService aesEncryptionService,
        JwtSecurityTokenHandler jwtSecurityTokenHandler)
        : TokenHandler
    {
        public override Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
        {
            var decodeToken = aesEncryptionService.Decrypt(token);
            return jwtSecurityTokenHandler.ValidateTokenAsync(decodeToken, validationParameters);
        }
    }
}