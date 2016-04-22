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
        public byte[] File { get; set; }
        internal Dictionary<string, Object> dictionary = new Dictionary<string, object>();
        public CloudFile(byte[] file) : this(file, null) { }
        public CloudFile(byte[] file, string fileName) : this(file, fileName, null) { }
        public CloudFile(byte[] file, string fileName, string contentType)
        {
            dictionary.Add("_id", null);
            dictionary.Add("_type", "file");
            dictionary.Add("ACL", new CB.ACL());
            dictionary.Add("name", fileName);
            dictionary.Add("contentType", contentType);
            dictionary.Add("size", null);
            dictionary.Add("expires", null);
            dictionary.Add("url", null);
            dictionary.Add("file", file);
        }

        public string ID
        {
            get
            {
                return dictionary["_id"].ToString();
            }
            set
            {
                dictionary["_id"] = value;
            }
        }

        public string Url
        {
            get
            {
                return dictionary["url"].ToString();
            }
            set
            {
                dictionary["url"] = value;
            }
        }
        public string FileName
        {
            get
            {
                return dictionary["name"].ToString();
            }
            set
            {
                dictionary["name"] = value;
            }
        }

        public string ContentType
        {
            get
            {
                return dictionary["contentType"].ToString();
            }
            set
            {
                dictionary["contentType"] = value;
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
                {
                    dictionary["ACL"] = value;
                }
                else
                    throw new Exception.CloudBoostException("Value is not of type ACL");
            }
        }
        public string Size
        {
            get
            {
                return dictionary["size"].ToString();
            }
            set
            {
                dictionary["size"] = value;
            }
        }

        public async Task<CloudFile> SaveAsync()
        {

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var url = CloudApp.ApiUrl + "/file/" + CloudApp.AppID;

            var result = Util.CloudRequest.SendFile(Util.CloudRequest.Method.PUT, url, this);

            this.dictionary = (Dictionary<string, Object>) await result;

            return this;
        }

        public async Task<CloudFile> DeleteAsync()
        {
            if(this.Url == null)
            {
                throw new CB.Exception.CloudBoostException("You cannot delete a file which does not have an URL");
            }
            Dictionary<string, Object> postData = new Dictionary<string, object>();

            postData.Add("fileObj", this);

            var url = CloudApp.ApiUrl + "/file/" + CloudApp.AppID + "/" + this.ID;

            var result = await Util.CloudRequest.SendObject(Util.CloudRequest.Method.DELETE, url, postData, false);

            this.dictionary = (Dictionary<string, Object>)result;
            this.Url = null;
            return this;
        }

        public async Task<Object> GetFileContentAsync()
        {
            if (this.Url == null)
            {
                throw new CB.Exception.CloudBoostException("You cannot delete a file which does not have an URL");
            }

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            var url = CloudApp.ApiUrl + "/file/" + CloudApp.AppID + "/" + this.ID;
            var result = await Util.CloudRequest.SendObject(Util.CloudRequest.Method.GET, this.Url, postData, false);
            return result;
        }
    }
}
