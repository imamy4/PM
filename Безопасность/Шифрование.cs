using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;

namespace Безопасность
{
    public static class Шифрование
    {
        private static MD5 md5 = MD5.Create();

        public static byte[] ПолучитьХешMD5(string текст, Encoding кодировка)
        {
            byte[] массивБайт = кодировка.GetBytes(текст);
            return md5.ComputeHash(массивБайт);
        }

        public static byte[] ПолучитьХешMD5(string текст)
        {
            return ПолучитьХешMD5(текст, Encoding.UTF8);
        }

        public static string ПолучитьХешMD5Строкой(string текст, Encoding кодировка)
        {
            string результат = string.Empty;

            byte[] хеш = ПолучитьХешMD5(текст, кодировка);
            foreach (byte b in хеш)
                результат += string.Format("{0:x2}", b);

            return результат;
        }

        public static string ПолучитьХешMD5Строкой(string текст)
        {
            return ПолучитьХешMD5Строкой(текст, Encoding.UTF8);
        }
    }
}
