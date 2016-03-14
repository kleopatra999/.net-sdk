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
            InitAppID();
            CloudApp.AppKey = "/W1GJ3g9Tju8KX78gcfcdiCMJNIDxtLy/VI5D3qH59g=";
        }

        public static void InitWithClientKey()
        {
            InitAppID();
            CloudApp.AppKey = "SCisM9B5TjaYPJuVuXdBcA==";
        }

        public static void InitAppID()
        {
            CloudApp.AppID = "table";
        }
    }
}
