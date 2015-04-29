using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CB.Util
{
    class CloudRequest
    {
        internal static async Task<Dictionary<string, Object>> GET(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(CloudApp.ApiUrl + url);
            var response = await request.GetResponseAsync();
            var responseString = new StreamReader(((HttpWebResponse)response).GetResponseStream()).ReadToEnd();
            return Util.Serializer.Deserialize(JObject.Parse(responseString));
        }

        internal static async Task<Dictionary<string, Object>> POST(string url, Dictionary<string, Object> postData)
        {
            var jsonObj = Util.Serializer.Serialize(postData);
            var parentJsonObj = new JObject();
            parentJsonObj["document"] = parentJsonObj;
            parentJsonObj["key"] = CloudApp.AppKey;

            var request = (HttpWebRequest)WebRequest.Create(CloudApp.ApiUrl + "/" + CloudApp.AppID + "/"+ url);

            var data = Encoding.ASCII.GetBytes(parentJsonObj.ToString());

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = await request.GetResponseAsync();

            var responseString = new StreamReader(((HttpWebResponse)response).GetResponseStream()).ReadToEnd();

            return Util.Serializer.Deserialize(JObject.Parse(responseString));
        }
    }
}
