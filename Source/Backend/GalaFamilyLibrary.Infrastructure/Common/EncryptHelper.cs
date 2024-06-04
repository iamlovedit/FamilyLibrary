using System.Security.Cryptography;
using System.Text;

namespace GalaFamilyLibrary.Infrastructure.Common;

public static class EncryptHelper
{
    public static string EncryptMD5(this byte[] bytes)
    {
        return GenerateMD5(bytes);
    }

    public static string MD5Encrypt32(this string source, string salt)
    {
        var contentBytes = Encoding.UTF8.GetBytes(source + salt);
        return GenerateMD5(contentBytes);
    }

    private static string GenerateMD5(byte[] bytes)
    {
        var buffer = MD5.Create().ComputeHash(bytes);
        var strBuilder = new StringBuilder();
        foreach (var item in buffer)
        {
            strBuilder.Append(item.ToString("x2"));
        }

        return strBuilder.ToString();
    }

    private static readonly string aesKey = "mkvciueszponuzwe";
    private static readonly byte[] aesIV = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

    public static string ExcryptAES(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
        }

        return ExcryptAES(source, aesKey);
    }

    private static string ExcryptAES(string source, string key)
    {
        if (string.IsNullOrEmpty(source))
        {
            throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
        }

        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));
        }
        var sourceArray = Encoding.UTF8.GetBytes(source);
        var keyArray = Encoding.UTF8.GetBytes(key);
        using (var symmetricKey = Aes.Create())
        {
            symmetricKey.Mode = CipherMode.CBC;
            symmetricKey.Padding = PaddingMode.PKCS7;
            using (var encryptor = symmetricKey.CreateEncryptor(keyArray, aesIV))
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(sourceArray, 0, sourceArray.Length);
                        cryptoStream.FlushFinalBlock();
                        var cipherTextBytes = memoryStream.ToArray();
                        return Convert.ToBase64String(cipherTextBytes);
                    }
                }
            }
        }
    }

    public static string? EncryptAES(string plainText, string passPhrase, string iv)
    {
        if (string.IsNullOrEmpty(plainText))
        {
            throw new ArgumentException($"“{nameof(plainText)}”不能为 null 或空。", nameof(plainText));
        }

        if (string.IsNullOrEmpty(passPhrase))
        {
            throw new ArgumentException($"“{nameof(passPhrase)}”不能为 null 或空。", nameof(passPhrase));
        }

        if (string.IsNullOrEmpty(iv))
        {
            throw new ArgumentException($"“{nameof(iv)}”不能为 null 或空。", nameof(iv));
        }

        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        var keyBytes = Encoding.UTF8.GetBytes(passPhrase);
        var ivBytes = Encoding.UTF8.GetBytes(iv);

        using (var symmetricKey = Aes.Create())
        {
            symmetricKey.Mode = CipherMode.CBC;
            symmetricKey.Padding = PaddingMode.PKCS7;
            using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivBytes))
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        var cipherTextBytes = memoryStream.ToArray();
                        return Convert.ToBase64String(cipherTextBytes);
                    }
                }
            }
        }
    }

    /// <remarks>https://stackoverflow.com/questions/69911084/problem-updating-to-net-6-encrypting-string</remarks>
    public static string? DecryptAES(string cipherText, string passPhrase, string iv)
    {
        if (string.IsNullOrEmpty(cipherText))
        {
            throw new ArgumentException($"“{nameof(cipherText)}”不能为 null 或空。", nameof(cipherText));
        }

        if (string.IsNullOrEmpty(passPhrase))
        {
            throw new ArgumentException($"“{nameof(passPhrase)}”不能为 null 或空。", nameof(passPhrase));
        }

        if (string.IsNullOrEmpty(iv))
        {
            throw new ArgumentException($"“{nameof(iv)}”不能为 null 或空。", nameof(iv));
        }
        try
        {
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            var keyBytes = Encoding.UTF8.GetBytes(passPhrase);
            var ivBytes = Encoding.UTF8.GetBytes(iv);
            using (var symmetricKey = Aes.Create())
            {
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;
                using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivBytes))
                {
                    using (var memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        using (var plainTextReader = new StreamReader(cryptoStream))
                        {
                            return plainTextReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            return null;
        }

    }

    public static string DecryptAES(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
        }
        return DecryptAES(source, aesKey);
    }


    private static string DecryptAES(string source, string key)
    {
        if (string.IsNullOrEmpty(source))
        {
            throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
        }

        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));
        }

        var keyArray = Encoding.UTF8.GetBytes(key);
        var inputByteArray = Convert.FromBase64String(source);
        using (var symmetricKey = Aes.Create())
        {
            symmetricKey.Mode = CipherMode.CBC;
            symmetricKey.Padding = PaddingMode.PKCS7;
            using (var decryptor = symmetricKey.CreateDecryptor(keyArray, aesIV))
            {
                using (var memoryStream = new MemoryStream(inputByteArray))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        var plainTextBytes = new byte[inputByteArray.Length];
                        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                    }
                }
            }
        }
    }
}