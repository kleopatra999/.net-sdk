//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace CB.Test
//{
//    [TestClass]
//    public class CloudCache
//    {
//        [TestMethod]
//        public void x001_InitAppWithMasterKey()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            Assert.IsTrue(true);
//        }

//        [TestMethod]
//        public async Task AddItemToCache()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            Dictionary<string, object> data = new Dictionary<string, object>();
//            data.Add("name", "Ranjeet");
//            data.Add("sex", "male");
//            data.Add("age", 24);   
//            var cache = new CB.CloudCache("student");
//            await cache.SetAsync("test1", data);
//            Assert.IsTrue(true);
//        }

//        [TestMethod]
//        public async Task AddString()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var cache = new CB.CloudCache("student");
//            var response = await cache.SetAsync("test1", "sample");
//            if (response.ToString() == "sample")
//            {
//                Assert.IsTrue(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task AddNumber()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var cache = new CB.CloudCache("student");
//            var response = await cache.SetAsync("test1", 1);
//            if ((int)response == 1)
//            {
//                Assert.IsTrue(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task DeleteItem()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var cache = new CB.CloudCache("student");
//            var response = await cache.SetAsync("test1", 1);
//            if ((int)response == 1)
//            {
//                response = await cache.DeleteItemAsync("test1");
//                if (response.ToString() == "test1")
//                {
//                    response = await cache.GetAsync("test1");
//                    if (response == null)
//                    {
//                        Assert.IsTrue(true);
//                    }
//                    Assert.IsFalse(true);
//                }
//                Assert.IsFalse(true);
//            }
//        }

//        [TestMethod]
//        public async Task CreateCahce()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var cache = new CB.CloudCache("student");
//            var response = await cache.CreateAsync();
//            if(response.ToString() == "student")
//            {
//                 Assert.IsTrue(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task GetItemCount()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            Dictionary<string, object> data = new Dictionary<string, object>();
//            data.Add("name", "Ranjeet");
//            data.Add("sex", "male");
//            data.Add("age", 24);   
//            var cache = new CB.CloudCache("student");
//            var result = await cache.SetAsync("test1", data);
//            data = (Dictionary<string, Object>)result;
//            if (data["name"].ToString() == "Ranjeet" && data["sex"].ToString() == "male" && (int)data["age"] == 24)
//            {
//                var count = await cache.GetItemsCountAsync();
//                if ((int)count >= 1)
//                {
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//        }

//        [TestMethod]
//        public async Task GetItemInTheCache()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            Dictionary<string, object> data = new Dictionary<string, object>();
//            data.Add("name", "Ranjeet");
//            data.Add("sex", "male");
//            data.Add("age", 24);  
//            var cache = new CB.CloudCache("student");
//            var response = await cache.SetAsync("test1", data);
//            data = (Dictionary<string, Object>)response;
//            if (data["name"].ToString() == "Ranjeet")
//            {
//                Assert.IsTrue(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task GetAllCacheItem()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            Dictionary<string, object> data = new Dictionary<string, object>();
//            Dictionary<string, object> result = new Dictionary<string, object>();
//            data.Add("name", "Ranjeet");
//            data.Add("sex", "male");
//            data.Add("age", 24);
//            var cache = new CB.CloudCache("student");
//            var response = await cache.SetAsync("test1", data);
//            result = (Dictionary<string, Object>)response;
//            if (result["name"].ToString() == "Ranjeet" && result["sex"].ToString() == "male" && (int)result["age"] == 24)
//            {
//                data["name"] = "sample2";
//                response = await cache.SetAsync("test1", data);
//                result = (Dictionary<string, Object>)response;
//                if (result["name"].ToString() == "sample2" && result["sex"].ToString() == "male" && (int)result["age"] == 24)
//                {
//                    response = await CB.CloudCache.GetAllAsync();
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//        }

//        [TestMethod]
//        public async Task GetInformationAboutCache()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            Dictionary<string, object> data = new Dictionary<string, object>();
//            Dictionary<string, object> result = new Dictionary<string, object>();
//            data.Add("name", "Ranjeet");
//            data.Add("sex", "male");
//            data.Add("age", 24);
//            var cache = new CB.CloudCache("student");
//            var response = await cache.SetAsync("test1", data);
//            result = (Dictionary<string, Object>)response;
//            if (result["name"].ToString() == "Ranjeet" && result["sex"].ToString() == "male" && (int)result["age"] == 24)
//            {
//                response = await cache.GetInfoAsync();
//                CB.CloudCache obj = (CB.CloudCache)response;
//                string size = obj.Size.ToString();
//                if (size.Contains("kb"))
//                {
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//        }
//    }
//}
