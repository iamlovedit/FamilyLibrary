using Microsoft.AspNetCore.DataProtection;

namespace GalaFamilyLibrary.Infrastructure.Security.Encyption
{
    public class DataProtectionHelper(IDataProtectionProvider dataProtectionProvider) : IDataProtectionHelper
    {
        public string Decrypt(string cipherText, string key)
        {
            return dataProtectionProvider.CreateProtector(key).Unprotect(cipherText);
        }

        public string Encrypt(string textToEncrypt, string key)
        {
            return dataProtectionProvider.CreateProtector(key).Protect(textToEncrypt);
        }
    }
}
