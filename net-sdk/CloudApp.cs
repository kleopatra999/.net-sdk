using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB
{

    public delegate void Callback(Object result);

    public class CloudApp
    {
        private static string serverUrl;
        
        public static string ServerUrl
        {
            get
            {
                if (serverUrl != null)
                {
                    return serverUrl;
                }
                else
                {
                    return @"https://api.cloudboost.io";
                }
            }
            set { serverUrl = value; }
        }
        public static string AppID { get; set; }
        public static string AppKey { get; set; }
        public static string ApiUrl
        {
            get
            {
                return serverUrl+"/api";
            }
        }

        public static void init(string appId, string appKey)
        {
            AppID = appId;
            AppKey = appKey;
        }

        public static void init(string serverUrl, string appId, string appKey)
        {
            if (serverUrl.EndsWith("/"))
            {
                serverUrl = serverUrl.TrimEnd('/');
            }

            ServerUrl = serverUrl;
            AppID = appId;
            AppKey = appKey;
        }


        public static bool Validate()
        {
            if (String.IsNullOrEmpty(AppID) || String.IsNullOrEmpty(AppKey) || String.IsNullOrEmpty(ApiUrl))
            {
                throw new Exception.CloudBoostException("AppID / AppKey OR API URL is missing.");
            }

            return true;
        }
    }
}
