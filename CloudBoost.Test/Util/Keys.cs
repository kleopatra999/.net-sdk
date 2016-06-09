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
            CloudApp.AppKey = "e8b6be17-1bc6-4d54-a141-1a67b647851a";
        }

        public static void InitWithClientKey()
        {
            InitAppID();
            CloudApp.AppKey = "312e7982-32f6-46a9-9f17-fa38ac4587e0";
        }

        public static void InitAppID()
        {
            CloudApp.AppID = "jmbcerpdfnbp";
        }
    }
}
