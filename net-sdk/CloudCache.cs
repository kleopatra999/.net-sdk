using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB
{
    class CloudCache
    {
        protected Dictionary<string, Object> dictionary = new Dictionary<string, object>();
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

        public Object Get(string name)
        { 
            return dictionary[name];
        }

        public async Task<CloudCache> SetAsync(string key, Object value)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);
            postData.Add("item", value);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/" + key;

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.PUT, url, postData, true);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudCache> SetAsync(string key, Object value)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);
            postData.Add("item", value);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/" + key;

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.PUT, url, postData, true);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudCache> DeleteItemAsync(string key)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/item/" + key;

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.DELETE, url, postData, true);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudCache> CreateAsync(string key)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/create";

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, true);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudCache> GetAsync(string key)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + key + "/item";

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, true);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudCache> GetInfoAsync(string key)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"];

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, true);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudCache> GetItemsCountAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/items/count";

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, true);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudCache> GetAllAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/items";

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, true);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudCache> ClearAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/clear";

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, true);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudCache> DeleteAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID + "/" + dictionary["name"];

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.DELETE, url, postData, true);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public static async Task<CloudCache> GetAllAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID;

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, true);

            CloudCache obj = (Dictionary<string, Object>)result;

            return obj;
        }

        public static async Task<CloudCache> DeleteAllAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("key", CB.CloudApp.AppKey);

            var url = CB.CloudApp.ApiUrl + "/cache/" + CB.CloudApp.AppID;

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.DELETE, url, postData, true);

            CloudCache obj = (Dictionary<string, Object>)result;

            return obj;
        }

    }
}
