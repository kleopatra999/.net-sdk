using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CB.Util
{
    class CloudRequest
    {
        /// <summary>
        /// Method of the HTTP Request
        /// </summary>
        public enum Method
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        /// <summary>
        /// Validates if the App ID and App Key is present. 
        /// </summary>
        /// <returns>Returns true if the app is valid, otherwise returns false.</returns>
        internal static bool Validate()
        {
            if (String.IsNullOrEmpty(CB.CloudApp.AppID) || String.IsNullOrEmpty(CB.CloudApp.AppKey))
            {
                throw new Exception.CloudBoostException("AppID or AppKey is missing.");
            }

            if (String.IsNullOrEmpty(CB.CloudApp.ApiUrl))
            {
                throw new Exception.CloudBoostException("API URL is missing.");
            }

            return true;
        }

        internal static async Task<T> Send<T>(Method method, string url, Dictionary<string, Object> postData)
        {
            try
            {
                CB.Util.CloudRequest.Validate();

                var request = (HttpWebRequest)WebRequest.Create(url);

                if (method == Method.POST || method == Method.PUT || method == Method.DELETE)
                {
                    if (postData == null)
                        postData = new Dictionary<string, object>();

                    postData.Add("key", CloudApp.AppKey);
                    var jsonObj = Util.Serializer.Serialize(postData);
                    var data = Encoding.ASCII.GetBytes(jsonObj.ToString());
                    request.Method = method.ToString();
                    request.ContentType = "application/json";
                    request.ContentLength = data.Length;
                }

                var response = await request.GetResponseAsync();

                var responseString = new StreamReader(((HttpWebResponse)response).GetResponseStream()).ReadToEnd();

                if (responseString.GetType() == typeof(Dictionary<string, Object>))
                {
                    object val = Util.Serializer.Deserialize(responseString);
                    return (T)Convert.ChangeType(val, typeof(T));
                }

                else if (responseString.GetType() == typeof(List<Dictionary<string, Object>>))
                {
                    object val = Util.Serializer.DeserializeArrayType(responseString);
                    return (T)Convert.ChangeType(val, typeof(T));
                }
                else {
                    object val = Util.Serializer.Serialize(null);
                    return (T)Convert.ChangeType(val, typeof(T));
                }
            }
            catch (System.Exception e)
            {
                throw new CB.Exception.CloudBoostException(e.ToString());
            }
        }
        
      
    }
}
