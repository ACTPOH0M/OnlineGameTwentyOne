using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace game_cards.Login_register
{
    class HashPassword
    {
        public static string hasPassword(string data)
        {
            MD5 md5 =  MD5.Create();

            byte[] b = Encoding.ASCII.GetBytes(data);
            byte[] hash = md5.ComputeHash(b);
            StringBuilder stringBuilder = new StringBuilder();
            foreach(var a in hash)
            {
                stringBuilder.Append(a.ToString("X2"));
            }

            return Convert.ToString(stringBuilder) ;
        }
    }
}
