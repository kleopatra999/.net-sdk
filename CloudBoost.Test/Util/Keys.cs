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
            CloudApp.AppKey = "aedb79b1-dbf1-44b5-8359-39496db1df87";
        }

        public static void InitWithClientKey()
        {
            InitAppID();
            CloudApp.AppKey = "cfc1c255-6bfe-49e6-a46e-26906107fc4e";
        }

        public static void InitAppID()
        {
            CloudApp.AppID = "ophskdrnsghu";
        }
    }
}
