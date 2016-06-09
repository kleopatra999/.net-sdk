using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB.Test.Util
{
    class Methods
    {
        internal static String MakeString()
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

        internal static String MakeEmail()
        {
            string email = CB.Test.Util.Methods.MakeString() + "@abc.com";
            return email;
        }
    }
}
