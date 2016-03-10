using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace CB
{
    public class CloudFile
    {
        internal Dictionary<string, Object> dictionary = new Dictionary<string,object>();

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

        public string ID
        {
            get
            {
                return (string)dictionary["_id"];
            }
        }

        public string Name
        {
            get
            {
                return (string)dictionary["name"];
            }
            set
            {
                dictionary["name"] = value;
            }
        }

        public string Size
        {
            get
            {
                return (string)dictionary["size"];
            }
            set
            {
                dictionary["size"] = value;
            }
        }

        public string Url
        {
            get
            {
                return (string)dictionary["url"];
            }
        }

        public CB.ACL ACL
        {
            get
            {
                return (CB.ACL)dictionary["ACL"];
            }
            set
            {
                if (value.GetType() == typeof(CB.ACL))
                    dictionary["ACL"] = value;
                else
                    throw new Exception.CloudBoostException("Value is not of type ACL");
            }
        }

        public Object Get(string name)
        {
            return dictionary[name];
        }

        public void Set(string name, Object data)
        {
            dictionary.Add(name, data);
        }

        public async Task<CloudFile> SaveAsync()
        {

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var url = CloudApp.ApiUrl + "/data/" + CloudApp.AppID + "/" + postData["_tableName"];

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.PUT, url, postData, false);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudFile> DeleteAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();

            postData.Add("document", this);

            var url = CloudApp.ApiUrl + "/data/" + CloudApp.AppID + "/" + postData["_tableName"];

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.DELETE, url, postData, false);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudFile> FetchAsync()
        {
            var query = new CB.CloudQuery("_File");
            query.EqualTo("id", this.ID);
            query.Limit(1);
            var result = (List<CloudObject>)await query.Find();
            this.dictionary = result[0].dictionary;
            return this;
        }

        public async Task<Object> GetFileContentAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.GET, this.Url, postData, false);

            return result;
        }
    }
}
