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
        private static string apiUrl;
        
        public static string ApiUrl
        {
            get
            {
                if (apiUrl != null)
                {
                    return apiUrl;
                }
                else
                {
                    return @"https://api.cloudboost.io";
                }
            }
            set { apiUrl = value; }
        }

        private static string serviceUrl;

        public static string ServiceURL
        {
            get
            {
                if (serviceUrl != null)
                {
                    return serviceUrl;
                }
                else
                {
                    return @"https://service.cloudboost.io";
                }
            }
            set { serviceUrl = value; }
        }

        public static string AppID { get; set; }
        public static string AppKey { get; set; }

        public static void init(string appId, string appKey)
        {
            AppID = appId;
            AppKey = appKey;
        }

        public static void init(string apiUrl, string serviceUrl, string appId, string appKey)
        {
            if (apiUrl.EndsWith("/"))
            {
                apiUrl = apiUrl.TrimEnd('/');
            }

            ApiUrl = apiUrl;
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
