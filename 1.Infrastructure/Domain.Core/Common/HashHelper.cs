using System;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Core.Common
{
     public class HashHelper
    {
        /// <summary>
        /// 计算字符串的MD5值（默认编码）
        /// </summary>
        /// <param name="input">原始字符串</param>
        /// <returns>字符串的MD5值</returns>
        public static string GetMd5(string input)
        {
            var provider = new MD5CryptoServiceProvider();
            var buffer = Encoding.Default.GetBytes(input);
            var crypt = provider.ComputeHash(buffer);
            var md5 = BitConverter.ToString(crypt).Replace("-", "").ToUpper();
            return md5;
        }
        public static string GetMd5Hash(string input)
        {
            var md5provider = new MD5CryptoServiceProvider();
            var buffer = Encoding.UTF8.GetBytes(input);
            var md5Buffer = md5provider.ComputeHash(buffer);
            return BitConverter.ToString(md5Buffer).Replace("-", "");
        }

        public static string GetSha1Hash(string input)
        {
            var sha1Provider = new SHA1CryptoServiceProvider();
            var buffer = Encoding.UTF8.GetBytes(input);
            var sha1Buffer = sha1Provider.ComputeHash(buffer);
            return BitConverter.ToString(sha1Buffer).Replace("-", "");
        }

        public static string GetHMACSha1Hash(string input,string secret)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1 { Key = Encoding.UTF8.GetBytes(secret) };
            byte[] dataBuffer = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }
        /// <summary>
        /// 计算字符串的MD5值(UFT-8编码) 32位大写
        /// </summary>
        /// <returns></returns>
        public static string GetMd5ForUTF8(string input)
        {
            var md5 = MD5.Create();//new MD5CryptoServiceProvider();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            var md5Str = BitConverter.ToString(bytes).Replace("-", "").ToUpper();
            return md5Str;
        }
    }
}