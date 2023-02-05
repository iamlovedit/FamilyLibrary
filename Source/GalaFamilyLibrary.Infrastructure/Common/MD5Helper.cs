using System.Security.Cryptography;
using System.Text;

namespace GalaFamilyLibrary.Infrastructure.Common
{
    public class MD5Helper
    {

        public static string MD5Encrypt16(string password)
        {
            var md5 = MD5.Create();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(password)), 4, 8);
            t2 = t2.Replace("-", string.Empty);
            return t2;
        }


        public static string MD5Encrypt32(string password = "")
        {
            string pwd = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(password) && !string.IsNullOrWhiteSpace(password))
                {
                    var md5 = MD5.Create(); //实例化一个md5对像
                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
                    var buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                    // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
                    foreach (var item in buffer)
                    {
                        // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                        pwd = string.Concat(pwd, item.ToString("X2"));
                    }
                }
            }
            catch
            {
                throw new Exception($"错误的 password 字符串:{password}");
            }
            return pwd;
        }


        public static string MD5Encrypt64(string password)
        {
            // 实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            var md5 = MD5.Create();
            var s = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(s);
        }

        public static string Sha1(string str, string format = "x2")
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            var data = SHA1.Create().ComputeHash(buffer);
            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString(format));
            }
            return sb.ToString();
        }

        public static string Sha256(string str, string format = "x2")
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            var data = SHA256.Create().ComputeHash(buffer);
            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString(format));
            }
            return sb.ToString();
        }

        public static string EncryptString(string str)
        {
            var md5 = MD5.Create();
            var byteOld = Encoding.UTF8.GetBytes(str);
            var byteNew = md5.ComputeHash(byteOld);
            var sb = new StringBuilder();
            foreach (var b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

    }
}
