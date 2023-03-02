using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace GalaFamilyLibrary.Infrastructure.Common;

public class GalaTokenValidator : ISecurityTokenValidator
{
    public bool CanReadToken(string securityToken)
    {
        return true;
    }

    public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        var principal = new JwtSecurityTokenHandler().ValidateToken(securityToken, validationParameters, out validatedToken);
        return principal;
    }

    public bool CanValidateToken { get; }
    public int MaximumTokenSizeInBytes { get; set; }
}