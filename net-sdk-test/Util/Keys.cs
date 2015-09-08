using CB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB.Test.Util
{
    class Keys
    {
        public static void InitWithMasterKey()
        {
            CloudApp.AppKey = "Qopoy/kXd+6G734HsjQMqGPGOvwEJYmBG84lQawRmWM=";
        }

        public static void InitWithClientKey()
        {
            CloudApp.AppKey = "9SPxp6D3OPWvxj0asw5ryA==";
        }
    }
}
