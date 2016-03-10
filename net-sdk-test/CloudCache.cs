using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CB.Test
{
    [TestClass]
    public class CloudCache
    {
        [TestMethod]
        public void x001_InitAppWithMasterKey()
        {
            CB.Test.Util.Keys.InitWithMasterKey();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task AddItemToCache()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("name", "Ranjeet");
            data.Add("sex", "male");
            data.Add("age", 24);   
            var cache = new CB.CloudCache("student");
            await cache.SetAsync("test1", data);
            Assert.IsTrue(true);
        }

        public async Task addString()
        {
            var cache = new CB.CloudCache("student");
            var response = await cache.SetAsync("test1", "sample");
            if (response == "sample")
            {
                Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }

        public async Task addNumber()
        {
            var cache = new CB.CloudCache("student");
            var response = await cache.SetAsync("test1", 1);
            if ((int)response == 1)
            {
                Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }

        public async Task deleteItem()
        {
            var cache = new CB.CloudCache("student");
            var response = await cache.SetAsync("test1", 1);
            if ((int)response == 1)
            {
                response = await cache.DeleteItemAsync("test1");
                if (response == "test1")
                {
                    response = await cache.GetAsync("test1");
                    if (response == null)
                    {
                        Assert.IsTrue(true);
                    }
                    Assert.IsFalse(true);
                }
                Assert.IsFalse(true);
            }
        }

        public async Task createCahce()
        {
            var cache = new CB.CloudCache("student");
            var response = await cache.CreateAsync();
            if(response == "student")
            {
                 Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }

        public async Task getItemCount()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("name", "Ranjeet");
            data.Add("sex", "male");
            data.Add("age", 24);   
            var cache = new CB.CloudCache("student");
            var result = await cache.SetAsync("test1", data);
            data = (Dictionary<string, Object>)result;
            if (data["name"] == "Ranjeet" && data["sex"] == "male" && (int)data["age"] == 24)
            {
                var count = await cache.GetItemsCountAsync();
                if ((int)count >= 1)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
        }

        public async Task getItemInTheCache()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("name", "Ranjeet");
            data.Add("sex", "male");
            data.Add("age", 24);  
            var cache = new CB.CloudCache("student");
            var response = await cache.SetAsync("test1", data);
            data = (Dictionary<string, Object>)response;
            if (data["name"] == "Ranjeet")
            {
                Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }

        public async Task getAllCacheItem()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            Dictionary<string, object> result = new Dictionary<string, object>();
            data.Add("name", "Ranjeet");
            data.Add("sex", "male");
            data.Add("age", 24);
            var cache = new CB.CloudCache("student");
            var response = await cache.SetAsync("test1", data);
            result = (Dictionary<string, Object>)response;
            if (result["name"] == "Ranjeet" && result["sex"] == "male" && (int)result["age"] == 24)
            {
                data["name"] = "sample2";
                response = await cache.SetAsync("test1", data);
                result = (Dictionary<string, Object>)response;
                if (result["name"] == "sample2" && result["sex"] == "male" && (int)result["age"] == 24)
                {
                    response = await CB.CloudCache.GetAllAsync();
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
        }

        public async Task getInformationAboutCache()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            Dictionary<string, object> result = new Dictionary<string, object>();
            data.Add("name", "Ranjeet");
            data.Add("sex", "male");
            data.Add("age", 24);
            var cache = new CB.CloudCache("student");
            var response = await cache.SetAsync("test1", data);
            result = (Dictionary<string, Object>)response;
            if (result["name"] == "Ranjeet" && result["sex"] == "male" && (int)result["age"] == 24)
            {
                response = await cache.GetInfoAsync();
                CB.CloudCache obj = (CB.CloudCache)response;
                string size = obj.Get("size").ToString();
                if (size.Contains("kb"))
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
        }

        public async Task deleteCacheFromAnApp()
        {

        }

        public async Task shouldThrewErrorWhileDeletingWrongData()
        {

        }

        public async Task shouldThrowErrorWhileClearingWrongData()
        {

        }

        public async Task clearCacheFromApp()
        {

        }

        public async Task deleteEntireCacheFromApp()
        {

        }
    }
}
