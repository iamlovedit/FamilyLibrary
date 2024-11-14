using Microsoft.IdentityModel.JsonWebTokens;

namespace GalaFamilyLibrary.Infrastructure.Security;

/// <summary>
/// 自定义token解密
/// </summary>
/// <param name="encryptionService"></param>
/// <param name="jwtTokenHandler"></param>
public class DefaultTokenHandler(IEncryptionService encryptionService, JsonWebTokenHandler jwtTokenHandler)
    : TokenHandler
{
    public override Task<TokenValidationResult> ValidateTokenAsync(string token,
        TokenValidationParameters validationParameters)
    {
        var decodeToken = encryptionService.Decrypt(token);
        return jwtTokenHandler.ValidateTokenAsync(decodeToken, validationParameters);
    }
}