namespace GalaFamilyLibrary.Infrastructure.Security
{
    public interface IAESEncryptionService
    {
        string Encrypt(string source, string? passPhrase = null, string? iv = null);

        string Decrypt(string source, string? passPhrase = null, string? iv = null);
    }
}
