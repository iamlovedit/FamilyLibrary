using Microsoft.AspNetCore.DataProtection;

namespace GalaFamilyLibrary.Infrastructure.Security.Encyption
{
    public class DataProtectionHelper : IDataProtectionHelper
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        public DataProtectionHelper(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }
        public string Decrypt(string cipherText, string key)
        {
            return _dataProtectionProvider.CreateProtector(key).Unprotect(cipherText);
        }

        public string Encrypt(string textToEncrypt, string key)
        {
            return _dataProtectionProvider.CreateProtector(key).Protect(textToEncrypt);
        }
    }
}
