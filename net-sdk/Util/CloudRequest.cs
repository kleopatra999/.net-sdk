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
                CB.PrivateMethods.Validate();

                var request = (HttpWebRequest)WebRequest.Create(url);

                if (method == Method.POST || method == Method.PUT || method == Method.DELETE)
                {
                    if (postData == null)
                        postData = new Dictionary<string, object>();

                    postData.Add("key", CloudApp.AppKey);
                    var jsonObj = Util.Serializer.Serialize(postData);
                    var data = Encoding.ASCII.GetBytes(jsonObj.ToString());
                    Console.WriteLine(data);
                    request.Method = method.ToString();
                    request.ContentType = "application/json";
                    request.ContentLength = data.Length;
                    /*using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }*/
                    using (Stream stream = await request.GetRequestStreamAsync())
                    {
                        byte[] byteArray = ASCIIEncoding.UTF8.GetBytes(jsonObj);
                        await stream.WriteAsync(byteArray, 0, byteArray.Length);
                        await stream.FlushAsync();
                        //stream.Write(data, 0, data.Length);
                    }
                }

                var response = await request.GetResponseAsync();

                var responseString = new StreamReader(((HttpWebResponse)response).GetResponseStream()).ReadToEnd();

                return Util.Serializer.Deserialize(responseString);
            }
            catch (System.Exception e)
            {
                CB.CloudApp.log.Error(".NET - CB.Util.CloudRequest.Send", e);
                throw e;
            }
        }

        internal static async Task<List<Dictionary<string, Object>>> SendArray(Method method, string url, Dictionary<string, Object> postData, Boolean isServiceUrl)
        {
            try
            {
                CB.PrivateMethods.Validate();

                var request = (HttpWebRequest)WebRequest.Create(url);

                if (method == Method.POST || method == Method.PUT || method == Method.DELETE)
                {
                    if (postData == null)
                        postData = new Dictionary<string, object>();

                    postData.Add("key", CloudApp.AppKey);
                    var jsonObj = Util.Serializer.Serialize(postData);
                    var data = Encoding.ASCII.GetBytes(jsonObj.ToString());
                    Console.WriteLine(data);
                    request.Method = method.ToString();
                    request.ContentType = "application/json";
                    request.ContentLength = data.Length;
                    using (Stream stream = await request.GetRequestStreamAsync())
                    {
                        byte[] byteArray = ASCIIEncoding.UTF8.GetBytes(jsonObj);
                        await stream.WriteAsync(byteArray, 0, byteArray.Length);
                        await stream.FlushAsync();
                        //stream.Write(data, 0, data.Length);
                    }
                }

                var response = await request.GetResponseAsync();

               var responseString = new StreamReader(((HttpWebResponse)response).GetResponseStream()).ReadToEnd();
                /*string responseString;
                using(var response = (HttpWebResponse)await request.GetResponseAsync());
                using (Stream streamResponse = response.GetResponseStream())
                using (StreamReader streamReader = new StreamReader(streamResponse))
                {
                    responseString = await streamReader.ReadToEndAsync();
                }*/
                return Util.Serializer.DeserializeArrayType(responseString);
                 
            }
            catch (System.Exception e)
            {
                CB.CloudApp.log.Error(".NET - CB.Util.CloudRequest.Send", e);
                throw e;
            }
        }

        internal static async Task<object> SendObject(Method method, string url, Dictionary<string, Object> postData, Boolean isServiceUrl)
        {
            try
            {
                CB.PrivateMethods.Validate();

                var request = (HttpWebRequest)WebRequest.Create(url);

                if (method == Method.POST || method == Method.PUT || method == Method.DELETE)
                {
                    if (postData == null)
                        postData = new Dictionary<string, object>();

                    postData.Add("key", CloudApp.AppKey);
                    var jsonObj = Util.Serializer.Serialize(postData);
                    var data = Encoding.ASCII.GetBytes(jsonObj.ToString());
                    Console.WriteLine(data);
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

                return responseString;
            }
            catch (System.Exception e)
            {
                CB.CloudApp.log.Error(".NET - CB.Util.CloudRequest.Send", e);
                throw e;
            }
        }

        internal static async Task<Dictionary<string, Object>> SendFile(Method method, string url, CB.CloudFile cf)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();

            postData.Add("key", CB.CloudApp.AppKey);
            postData.Add("sdk", "java");
            postData.Add("fileObj", cf);

            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postData, formDataBoundary);
            var response = await PostFile(method.ToString(), url, "Mozilla/5.0 (Windows NT 6.1; rv:26.0) Gecko/20100101 Firefox/26.0", contentType, formData);
            var responseString = new StreamReader(((HttpWebResponse)response).GetResponseStream()).ReadToEnd();
            return Util.Serializer.Deserialize(responseString);
        }

        internal static async Task<HttpWebResponse> PostFile(string method, string url, string userAgent, string contentType, byte[] formData)
        {
            try
            {
                CB.PrivateMethods.Validate();

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToString();
                request.UserAgent = userAgent;
                request.ContentType = contentType;
                request.CookieContainer = new CookieContainer();
                request.ContentLength = formData.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formData, 0, formData.Length);
                    requestStream.Close();
                }

                return await request.GetResponseAsync() as HttpWebResponse;
            }
            catch (System.Exception e)
            {
                CB.CloudApp.log.Error(".NET - CB.Util.CloudRequest.Send", e);
                throw e;
            }
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;
            ASCIIEncoding encoding = new ASCIIEncoding();
            
            foreach (var param in postParameters)
            {
                
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is CB.CloudFile)
                {
                    CB.CloudFile fileToUpload = (CB.CloudFile)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

    }
}
