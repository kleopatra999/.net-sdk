using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB.Test.Util
{
    class Methods
    {
        public static String _makeString()
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 8; i++)
            {
                char c = chars[random.Next(chars.Length)];
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static String _makeEmail()
        {
            string email = CB.Test.Util.Methods._makeString() + "@abc.com";

            return email;
        }
    }
}
