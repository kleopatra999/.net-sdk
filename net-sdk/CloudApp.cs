using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB
{
    public class CloudApp
    {
        private static string apiUrl;

        public static string AppID { get; set; }
        public static string AppKey { get; set; }
        public static string ApiUrl
        {
            get
            {
                if (apiUrl != null)
                {
                    return apiUrl;
                }
                else {
                    return @"https://api.cloudboost.io/api";
                }
            }
            set { apiUrl = value; }
        }

        public static void init(string appId, string appKey)
        {
            AppID = appId;
            AppKey = appKey;
        }

        public static void init(string apiUrl, string appId, string appKey)
        {
            if (apiUrl.EndsWith("/"))
            {
                apiUrl = apiUrl.TrimEnd('/');
            }

            ApiUrl = apiUrl+"/api";
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
