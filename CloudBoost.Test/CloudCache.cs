using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CB.Test
{
    [TestFixture]
    public class CloudCache
    {
        [Test]
        public void x001_InitAppWithMasterKey()
        {
            CB.Test.Util.Keys.InitWithMasterKey();
            Assert.IsTrue(true);
        }

        [Test]
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

        [Test]
        public async Task addString()
        {
            var cache = new CB.CloudCache("student");
            var response = await cache.SetAsync("test1", "sample");
            if (response.ToString() == "sample")
            {
                Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
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

        [Test]
        public async Task deleteItem()
        {
            var cache = new CB.CloudCache("student");
            var response = await cache.SetAsync("test1", 1);
            if ((int)response == 1)
            {
                response = await cache.DeleteItemAsync("test1");
                if (response.ToString() == "test1")
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

        [Test]
        public async Task createCahce()
        {
            var cache = new CB.CloudCache("student");
            var response = await cache.CreateAsync();
            if(response.ToString() == "student")
            {
                 Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task getItemCount()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("name", "Ranjeet");
            data.Add("sex", "male");
            data.Add("age", 24);   
            var cache = new CB.CloudCache("student");
            var result = await cache.SetAsync("test1", data);
            data = (Dictionary<string, Object>)result;
            if (data["name"].ToString() == "Ranjeet" && data["sex"].ToString() == "male" && (int)data["age"] == 24)
            {
                var count = await cache.GetItemsCountAsync();
                if ((int)count >= 1)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
        }

        [Test]
        public async Task getItemInTheCache()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("name", "Ranjeet");
            data.Add("sex", "male");
            data.Add("age", 24);  
            var cache = new CB.CloudCache("student");
            var response = await cache.SetAsync("test1", data);
            data = (Dictionary<string, Object>)response;
            if (data["name"].ToString() == "Ranjeet")
            {
                Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
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
            if (result["name"].ToString() == "Ranjeet" && result["sex"].ToString() == "male" && (int)result["age"] == 24)
            {
                data["name"] = "sample2";
                response = await cache.SetAsync("test1", data);
                result = (Dictionary<string, Object>)response;
                if (result["name"].ToString() == "sample2" && result["sex"].ToString() == "male" && (int)result["age"] == 24)
                {
                    response = await CB.CloudCache.GetAllAsync();
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
        }

        [Test]
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
            if (result["name"].ToString() == "Ranjeet" && result["sex"].ToString() == "male" && (int)result["age"] == 24)
            {
                response = await cache.GetInfoAsync();
                CB.CloudCache obj = (CB.CloudCache)response;
                string size = obj.Size.ToString();
                if (size.Contains("kb"))
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
        }

       
    }
}
