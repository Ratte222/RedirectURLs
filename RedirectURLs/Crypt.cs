using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RedirectURLs
{
    public class Crypt
    {

        /// <summary>
        ///
        /// </summary>
        /// <param name="value">сгенерировать хеш для этого значения</param>
        /// <returns>hash for message in Base64String</returns>
        public static string GetHashSHA512(string value)
        {
            byte[] val = Encoding.Default.GetBytes(value);
            return Convert.ToBase64String(InternalGetHashSHA512(val));

        }
        private static byte[] InternalGetHashSHA512(byte[] value)
        {
            byte[] hashValue = new byte[512];
            using (SHA512 mySHA512 = SHA512.Create())
            {
                hashValue = mySHA512.ComputeHash(value);
            }
            return hashValue;
        }
    }
}
