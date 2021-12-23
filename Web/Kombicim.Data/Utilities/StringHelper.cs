using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kombicim.Data.Utilities
{
    public class StringHelper
    {
        public const int GUID_LENGTH = 8;

        public static string Random(int length, bool uppercase = false)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
                stringChars[i] = chars[random.Next(chars.Length)];

            var str = new string(stringChars);
            if (uppercase)
                str = str.ToUpperInvariant();

            return str;
        }

        public static string Hash(string text)
        {
            using (var sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (var b in hash)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }
    }
}
