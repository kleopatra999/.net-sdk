using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;

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
                    postData.Add("sdk", ".NET");
                    var jsonObj = JsonConvert.SerializeObject(Serializer.Serialize(postData));
                    byte[] byteArray = Encoding.UTF8.GetBytes(jsonObj);
                    request.Method = method.ToString();
                    request.ContentType = "application/json";
                    request.ContentLength = byteArray.Length;

                    if (CloudApp.Session!=null)
                    {
                        request.Headers.Add("sessionID", CloudApp.Session);
                    }

                    using (Stream stream = await request.GetRequestStreamAsync())
                    {
                        await stream.WriteAsync(byteArray, 0, byteArray.Length);
                        await stream.FlushAsync();
                    }
                }

                var response = await request.GetResponseAsync();

                if (response.Headers["sessionID"]!=null)
                {
                    CloudApp.Session = response.Headers["sessionID"];
                }

                var responseString = JsonConvert.DeserializeObject<object>(new StreamReader(((HttpWebResponse)response).GetResponseStream()).ReadToEnd());

                if (responseString == null)
                    return default(T);

                object val = Util.Serializer.Deserialize(responseString);

                try
                {
                    return (T)val;    
                }
                catch (System.Exception e)
                {
                    return (T)Convert.ChangeType(val, typeof(T));
                }
            }
            catch (WebException e)
            {
                string text = "";

                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    text = httpResponse.StatusDescription.ToString();
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        text += " : "+reader.ReadToEnd();
                    }
                }

                throw new CB.Exception.CloudBoostException(text);
            }
            catch (System.Exception e)
            {
                throw new CB.Exception.CloudBoostException(e.Message.ToString());
            }
        }
        
      
    }
}
