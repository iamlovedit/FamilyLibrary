namespace GalaFamilyLibrary.Infrastructure.Security.Encyption
{
    public interface IAESEncryptionService
    {
        string Encrypt(string source, string? passPhrase = null, string? iv = null);

        string Decrypt(string source, string? passPhrase = null, string? iv = null);
    }
}
