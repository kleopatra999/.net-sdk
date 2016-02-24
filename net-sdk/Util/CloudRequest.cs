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
        public enum Method
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        internal static async Task<Dictionary<string, Object>> Send(Method method, string url, Dictionary<string, Object> postData, Boolean isServiceUrl)
        {
            try
            {
                CloudApp.Validate();

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
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                var response = await request.GetResponseAsync();

                var responseString = new StreamReader(((HttpWebResponse)response).GetResponseStream()).ReadToEnd();

                return Util.Serializer.Deserialize(JObject.Parse(responseString));
            }
            catch (System.Exception e)
            {
                CB.CloudApp.log.Error(".NET - CB.Util.CloudRequest.Send", e);
                throw e;
            }
        }
    }
}
