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
            CloudApp.AppKey = "2a30e91b-9492-44c3-b52f-8b0c6576c6e2";
        }

        public static void InitWithClientKey()
        {
            InitAppID();
            CloudApp.AppKey = "ea992f8c-497c-4ab2-a430-32e9ef26f515";
        }

        public static void InitAppID()
        {
            CloudApp.AppID = "ophskdrnsghu";
        }
    }
}
