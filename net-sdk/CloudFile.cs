using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace CB
{
    class CloudFile
    {
        protected Dictionary<string, Object> dictionary = new Dictionary<string,object>();

        public CloudFile(string name, byte[] data, string contentType = null)
        {
            dictionary.Add("_id", null);
            dictionary.Add("_type", "file");
            dictionary.Add("ACL", new CB.ACL());
            dictionary.Add("name", name);
            dictionary.Add("contentType", contentType);
            dictionary.Add("size", null);
            dictionary.Add("expires", null);
            dictionary.Add("url", null);
            dictionary.Add("file", new MemoryStream(data));
        }

        public CloudFile(string name, Stream data, string contentType = null)
        {
            dictionary.Add("_id", null);
            dictionary.Add("_type", "file");
            dictionary.Add("ACL", new CB.ACL());
            dictionary.Add("name", name);
            dictionary.Add("contentType", contentType);
            dictionary.Add("size", null);
            dictionary.Add("expires", null);
            dictionary.Add("url", null);
            dictionary.Add("file", data);
        }

        public CloudFile(string name, string url, string contentType = null)
        {
            dictionary.Add("_id", null);
            dictionary.Add("_type", "file");
            dictionary.Add("ACL", new CB.ACL());
            dictionary.Add("name", name);
            dictionary.Add("contentType", contentType);
            dictionary.Add("size", null);
            dictionary.Add("expires", null);
            dictionary.Add("url", url);
            dictionary.Add("file", null);
        }
        public Object Get(string name)
        {
            return dictionary[name];
        }

        public void Set(string name, Object data)
        {
            dictionary.Add(name, data);
        }

        public async Task SaveAsync()
        {

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var url = CloudApp.ApiUrl + "/data/" + CloudApp.AppID + "/" + postData["_tableName"];

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.PUT, url, postData, false);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task DeleteAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();

            postData.Add("document", this);

            var url = CloudApp.ApiUrl + "/data/" + CloudApp.AppID + "/" + postData["_tableName"];

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.DELETE, url, postData, false);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }
    }
}
