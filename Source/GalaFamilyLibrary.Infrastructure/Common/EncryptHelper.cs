using System.Security.Cryptography;
using System.Text;
using Ocelot.Cache;

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
}