using Microsoft.AspNetCore.DataProtection;

namespace GalaFamilyLibrary.Infrastructure.Common;

public class DataProtectionHelper
{
    private readonly IDataProtectionProvider _dataProtectionProvider;

    public DataProtectionHelper(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtectionProvider = dataProtectionProvider;
    }

    public string Encrypt(string textToEncrypt, string key)
    {
        return _dataProtectionProvider.CreateProtector(key).Protect(textToEncrypt);
    }

    public string Decrypt(string cipherText, string key)
    {
        return _dataProtectionProvider.CreateProtector(key).Unprotect(cipherText);
    }
}