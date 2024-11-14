namespace GalaFamilyLibrary.Infrastructure.Security;

/// <summary>
/// aes对称加解密服务
/// </summary>
public interface IEncryptionService
{
    string Encrypt(string plain, string? aesKey = null);

    string Decrypt(string cipher, string? aesKey = null);
}

public class EncryptionService(IConfiguration configuration) : IEncryptionService
{
    public string Encrypt(string plain, string? aesKey = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(plain);
        using var aes = CreateAes(aesKey);
        using var encryptor = aes.CreateEncryptor();
        var plainTextArray = Encoding.UTF8.GetBytes(plain);
        var resultArray = encryptor.TransformFinalBlock(plainTextArray, 0, plainTextArray.Length);
        var result = Convert.ToBase64String(resultArray);
        Array.Clear(resultArray);
        return result;
    }

    public string Decrypt(string cipher, string? aesKey = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(cipher);
        using var aes = CreateAes(aesKey);
        using var decryptor = aes.CreateDecryptor();
        var cipherTextArray = Convert.FromBase64String(cipher);
        var resultArray = decryptor.TransformFinalBlock(cipherTextArray, 0, cipherTextArray.Length);
        var result = Encoding.UTF8.GetString(resultArray);
        Array.Clear(resultArray);
        return result;
    }

    private Aes CreateAes(string? aesKey)
    {
        var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;
        var key = aesKey ?? configuration["AES_KEY"];
        ArgumentException.ThrowIfNullOrEmpty(key);
        aes.Key = MD5.HashData(Encoding.UTF8.GetBytes(key));
        return aes;
    }
}