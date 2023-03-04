using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.Extensions.Options;

namespace GalaFamilyLibrary.Infrastructure.Security.Encyption
{
    public class AESEncryptionService : IAESEncryptionService
    {
        private readonly AESEncryptionOption _encryptionOption;
        public AESEncryptionService(AESEncryptionOption aESEncryptionOption)
        {
            _encryptionOption = aESEncryptionOption;
        }

        public string Decrypt(string source, string? passPhrase = null, string? iv = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }
            if (string.IsNullOrEmpty(passPhrase))
            {
                passPhrase = _encryptionOption.SecretParameterEncryptKey;
            }
            if (!string.IsNullOrEmpty(iv))
            {
                iv = _encryptionOption.SecretParameterIV;
            }
            return EncryptHelper.DecryptAES(source, passPhrase, iv);
        }

        public string Encrypt(string source, string? passPhrase = null, string? iv = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }
            if (string.IsNullOrEmpty(passPhrase))
            {
                passPhrase = _encryptionOption.SecretParameterEncryptKey;
            }
            if (!string.IsNullOrEmpty(iv))
            {
                iv = _encryptionOption.SecretParameterIV;
            }
            return EncryptHelper.EncryptAES(source, passPhrase, iv);
        }
    }
}
