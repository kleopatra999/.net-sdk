using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB
{
    public class CloudCache
    {
        internal Dictionary<string, Object> dictionary = new Dictionary<string, object>();
        protected List<Object> items = new List<Object>();
        public CloudCache(string cacheName)
        {
            if (cacheName == " ")
            {
                throw new CB.Exception.CloudBoostException("Cannot create a cache with empty name");
            }

            dictionary.Add("_tableName", "cache");
            dictionary.Add("name", cacheName);
            dictionary.Add("size", "");
            dictionary.Add("items", items);

        }
        public string Name
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
        public Object Get(string name)
        { 
            return dictionary[name];
        }

        public async Task<object> SetAsync(string key, Object value)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);
            postData.Add("item", value);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/" + key;

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.PUT, url, postData);

            return result;
        }

        public async Task<CloudCache> DeleteItemAsync(string key)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/item/" + key;

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.DELETE, url, postData);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<object> CreateAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/create";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);

            return result;
        }

        public async Task<CloudCache> GetAsync(string key)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + key + "/item";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudCache> GetInfoAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"];

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<object> GetItemsCountAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/items/count";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);
            return result;
        }

        public static async Task<List<object>> GetAllAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID;

            var result = await Util.CloudRequest.Send<List<Dictionary<string, Object>>>(Util.CloudRequest.Method.POST, url, postData);

            List<object> list = new List<object>();
            for (int i = 0; i < result.Count; i++)
            {
                list.Add(result[i]);
            }
            
            return list;
        }

        public async Task<CloudCache> ClearAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/clear";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudCache> DeleteAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"];

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.DELETE, url, postData);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

       
        public static async Task<CloudCache> DeleteAllAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID;

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.DELETE, url, postData);

            Dictionary<string, object> dictionary = (Dictionary<string, Object>)result;
            var obj = new CloudCache(dictionary["name"].ToString());
            obj.dictionary = dictionary;

            return obj;
        }

    }
}
