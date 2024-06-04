namespace GalaFamilyLibrary.Infrastructure.Security.Encyption
{
    public interface IDataProtectionHelper
    {
        string Encrypt(string textToEncrypt, string key);

        string Decrypt(string cipherText, string key);
    }
}
