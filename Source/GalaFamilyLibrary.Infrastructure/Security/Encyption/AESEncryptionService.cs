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

            return EncryptHelper.DecryptAES(source, passPhrase ?? _encryptionOption.SecretParameterEncryptKey, iv ?? _encryptionOption.SecretParameterIV);
        }

        public string Encrypt(string source, string? passPhrase = null, string? iv = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }

            return EncryptHelper.EncryptAES(source, passPhrase ?? _encryptionOption.SecretParameterEncryptKey, iv ?? _encryptionOption.SecretParameterIV);
        }
    }
}
