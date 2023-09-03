namespace GalaFamilyLibrary.Infrastructure.Security.Encyption
{
    public interface IAESEncryptionService
    {
        string Encrypt(string source, string? aesKey = null);

        string Decrypt(string source, string? aesKey = null);
    }
}
