using System.Security.Cryptography;
using System.Text;

namespace GalaFamilyLibrary.UserService.Helpers
{
    public static class PasswordHelper
    {
        public static string MD5Encrypt32(this string source, string salt)
        {
            var buffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(source + salt));
            var strBuilder = new StringBuilder();
            foreach (var item in buffer)
            {
                strBuilder.Append(item.ToString("x2"));
            }
            return strBuilder.ToString();
        }
    }
}
