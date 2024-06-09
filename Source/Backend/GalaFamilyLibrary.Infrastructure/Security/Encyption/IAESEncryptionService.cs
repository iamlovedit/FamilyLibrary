using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace GalaFamilyLibrary.Infrastructure.Security.Encyption
{
    public interface IAESEncryptionService
    {
        string Encrypt(string source, string? aesKey = null);

        string Decrypt(string source, string? aesKey = null);
    }

    public class AESEncryptionService(IConfiguration configuration) : IAESEncryptionService
    {
        public string Decrypt(string source, string? aesKey = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("Cipher text can not be null or empty");
            }
            using var aes = CreateAes(aesKey);
            using var decryptor = aes.CreateDecryptor();
            var encryptTextArray = Convert.FromBase64String(source);
            var resultArray = decryptor.TransformFinalBlock(encryptTextArray, 0, encryptTextArray.Length);
            var result = Encoding.UTF8.GetString(resultArray);
            Array.Clear(resultArray, 0, resultArray.Length);
            resultArray = null;
            return result;
        }

        public string Encrypt(string source, string? aesKey = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("Plain text can not be null or empty");
            }

            using var aes = CreateAes(aesKey);
            using var encryptor = aes.CreateEncryptor();
            var plainTextArray = Encoding.UTF8.GetBytes(source);
            var resultArray = encryptor.TransformFinalBlock(plainTextArray, 0, plainTextArray.Length);
            var result = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            Array.Clear(resultArray, 0, resultArray.Length);
            resultArray = null;
            return result;
        }

        private Aes CreateAes(string aesKey)
        {
            using var md5 = MD5.Create();
            var aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            var key = aesKey ?? configuration["AES_KEY"];
            aes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key!));
            return aes;
        }
    }
}
