//using System;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace CB.Test
//{
//    [TestClass]
//    public class CloudQueue
//    {
//        [TestMethod]
//        public async Task NoQueueInDB()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            await CB.CloudQueue.GetAllAsync();
//            Assert.IsTrue(true);
//        }

//        [TestMethod]
//        public async Task GetMessageForFutureExpireDate()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var queueMessage = new CB.QueueMessage("data");
//            var tomorrow = new DateTime();
//            tomorrow.AddDays(1);
//            queueMessage.Expires = tomorrow;
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(queueMessage);
//            var response = await queue.AddMessageAsync(list);
//            var result = await queue.GetMessageAsync();
//            Assert.IsTrue(true);
//        }

//        [TestMethod]
//        public async Task AddDataIntoQueue()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            await queue.AddMessageAsync("sample");
//            Assert.IsTrue(true);

//        }

//        [TestMethod]
//        public async Task CreateAndDeleteQueue()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            await queue.CreateAsync();
//            await queue.DeleteAsync();
//            Assert.IsTrue(true);
//        }

//        [TestMethod]
//        public async Task AddExpireInQueue()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var queueMessage = new CB.QueueMessage("data");
//            var tomorrow = new DateTime();
//            tomorrow.AddDays(1);
//            queueMessage.Expires = tomorrow;
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(queueMessage);
//            var response = await queue.AddMessageAsync(list);
//            Assert.IsTrue(true);
//        }

//        [TestMethod]
//        public async Task AddCurrentTimeAsExpireDate()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var queueMessage = new CB.QueueMessage("data");
//            queueMessage.Expires = new DateTime();
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(queueMessage);
//            await queue.AddMessageAsync(list);
//            Assert.IsTrue(true);
//        }

//        [TestMethod]
//        public async Task UpdateDataIntoTheQueue()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var response = await queue.AddMessageAsync("sample");
//            if (response.Message.ToString() == "sample")
//            {
//                response.Message = "Hey!";
//                List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//                list.Add(response);
//                await queue.UpdateMessageAsync(list);
//                Assert.IsTrue(true);
//            }
//        }

//        [TestMethod]
//        public async Task AddMultipleMessageInQueue()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var queueMessage1 = new CB.QueueMessage("sample1");
//            var queueMessage2 = new CB.QueueMessage("sample2");
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(queueMessage1);
//            list.Add(queueMessage2);
//            var response = await queue.AddMessageAsync(list);
//            //should return array of queueMessage
//            Assert.IsTrue(true);
//        }

//        [TestMethod]
//        public async Task AddAndGetDataFromTheQueue()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var Message = new CB.QueueMessage("sample");
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(Message);
//            var response = await queue.AddMessageAsync(list);
//            if (response.Message.ToString() == "sample")
//            {
//                var result = await queue.GetMessageAsync();
//                if (result.Message.ToString() == "sample")
//                {
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task ShouldPeek()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var Message = new CB.QueueMessage("sample");
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(Message);
//            var response = await queue.AddMessageAsync(list);
//            if (response.Message.ToString() == "sample")
//            {
//                var result = await queue.PeekMessageAsync();
//                if (result.Message.ToString() == "sample")
//                {
//                    var result1 = await queue.PeekMessageAsync();
//                    if (result.Message.ToString() == "sample")
//                    {
//                        Assert.IsTrue(true);
//                    }
//                    Assert.IsFalse(true);
//                }
//                Assert.IsFalse(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task ShouldGetMessageInFIFO()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var Message = new CB.QueueMessage("sample1");
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(Message);
//            var response = await queue.AddMessageAsync(list);
//            if (response.Message.ToString() == "sample1")
//            {
//                Message = new CB.QueueMessage("sample2");
//                list = new List<CB.QueueMessage>();
//                list.Add(Message);
//                response = await queue.AddMessageAsync(list);
//                if (response.Message.ToString() == "sample2")
//                {
//                    response = await queue.GetMessageAsync();
//                    if (response.Message.ToString() == "sample1")
//                    {
//                        response = await queue.GetMessageAsync();
//                        if (response.Message.ToString() == "sample2")
//                        {
//                            Assert.IsTrue(true);
//                        }
//                        Assert.IsFalse(true);
//                    }
//                    Assert.IsFalse(true);
//                }
//                Assert.IsFalse(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task ShouldPeekTwoMessageSameTime()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var Message = new CB.QueueMessage("sample1");
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(Message);
//            var response = await queue.AddMessageAsync(list);
//            if (response.Message.ToString() == "sample1")
//            {
//                Message = new CB.QueueMessage("sample2");
//                list = new List<CB.QueueMessage>();
//                list.Add(Message);
//                response = await queue.AddMessageAsync(list);
//                if (response.Message.ToString() == "sample2")
//                {
//                    response = await queue.PeekMessageAsync(2);
//                    //should return array of QueueMessage
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task ShouldGetTwoMessageSameTime()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var Message = new CB.QueueMessage("sample1");
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(Message);
//            var response = await queue.AddMessageAsync(list);
//            if (response.Message.ToString() == "sample1")
//            {
//                Message = new CB.QueueMessage("sample2");
//                list = new List<CB.QueueMessage>();
//                list.Add(Message);
//                response = await queue.AddMessageAsync(list);
//                if (response.Message.ToString() == "sample2")
//                {
//                    response = await queue.GetMessageAsync(2);
//                    //should return array of QueueMessage
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task ShouldNotGetMessageWithDelay()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var Message = new CB.QueueMessage("sample");
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(Message);
//            var response = await queue.AddMessageAsync(list);
//            if (response.Message.ToString() == "sample1")
//            {
//                response = await queue.GetMessageAsync();
//                Assert.IsTrue(true);
//            }
//        }

//        [TestMethod]
//        public async Task ShouldAbleToGetMessageAfterDelay()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var Message = new CB.QueueMessage("sample");
//            Message.Delay = 1;
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(Message);
//            var response = await queue.AddMessageAsync(list);
//            if (response.Message.ToString() == "sample")
//            {
//                response = await queue.GetMessageAsync();
//                if (response.Message.ToString() == "sample")
//                {
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task ShouldGetMessageWithId()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var Message = new CB.QueueMessage("sample");
//            Message.Delay = 1;
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(Message);
//            var response = await queue.AddMessageAsync(list);
//            if (response.Message.ToString() == "sample")
//            {
//                response = await queue.GetMessageByID(response.ID);
//                if (response.Message.ToString() == "sample")
//                {
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task ShouldGetNullForInvalidMessageId()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var Message = new CB.QueueMessage("sample");
//            Message.Delay = 1;
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(Message);
//            var response = await queue.AddMessageAsync(list);
//            if (response.Message.ToString() == "sample")
//            {
//                response = await queue.GetMessageByID("sample");
//                if (response == null)
//                {
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task ShouldDeleteMessageWithMessageId()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var Message = new CB.QueueMessage("sample");
//            Message.Delay = 1;
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(Message);
//            var response = await queue.AddMessageAsync(list);
//            if (response.Message.ToString() == "sample")
//            {
//                var result = await queue.DeleteMessageAsync(response.ID);
//                if (result != null && result.ID == response.ID)
//                {
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task ShouldDeleteMessageByPassingQueueMessageToFunction()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var Message = new CB.QueueMessage("sample");
//            Message.Delay = 1;
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(Message);
//            var response = await queue.AddMessageAsync(list);
//            if (response.Message.ToString() == "sample")
//            {
//                var result = await queue.DeleteMessageAsync(response);
//                if (result != null && result.ID == response.ID)
//                {
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task ShouldNotGetMessageAfterItWasDeleted()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var Message = new CB.QueueMessage("sample");
//            Message.Delay = 1;
//            List<CB.QueueMessage> list = new List<CB.QueueMessage>();
//            list.Add(Message);
//            var response = await queue.AddMessageAsync(list);
//            if (response.Message.ToString() == "sample")
//            {
//                var result = await queue.DeleteMessageAsync(response);
//                if (result != null && result.ID == response.ID)
//                {
//                    var obj = queue.GetMessageByID(response.ID);
//                    if(obj == null)
//                        Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//            Assert.IsFalse(true);
//        }

//        [TestMethod]
//        public async Task ShouldAddSubscriberToQueue()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var url = "http://sample.sample.com";
//            List<object> list = new List<object>();
//            list.Add(url);
//            var response = await queue.AddSubscriberAsync(list);
//            for (int i = 0; i < list.Count; i++)
//            {
//                if (response.Subscribers.Contains(url[i]) == false)
//                {
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//        }

//        [TestMethod]
//        public async Task ShouldMultipleSubscriberToTheQueue()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var url = "http://sample.sample.com";
//            List<object> list = new List<object>();
//            list.Add(url);
//            var response = await queue.RemoveSubscriberAsync(list);
//            for (int i = 0; i < list.Count; i++)
//            {
//                if (response.Subscribers.Contains(url[i]) == false)
//                {
//                    Assert.IsTrue(true);
//                }
//                Assert.IsFalse(true);
//            }
//        }

//        [TestMethod]
//        public async Task ShouldRemoveSubscriberFromTheQueue()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var queue = new CB.CloudQueue(Util.Methods.MakeString());
//            var url = "http://sample.sample.com";
//            List<object> list = new List<object>();
//            list.Add(url);
//            var response = await queue.AddSubscriberAsync(list);
//            for (int i = 0; i < list.Count; i++)
//            {
//                if (response.Subscribers.Count == 1)
//                {
//                    var result = await queue.RemoveSubscriberAsync(list);
//                    if (response.Subscribers.Count == 0)
//                    {
//                        Assert.IsTrue(true);
//                    }
//                    Assert.IsFalse(true);
//                }
//                Assert.IsFalse(true);
//            }
//        }
//    }
//}
