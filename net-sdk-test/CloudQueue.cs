using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CB.Test
{
    [TestClass]
    public class CloudQueue
    {
        [TestMethod]
        public async Task noQueueInDB()
        {
            await CB.CloudQueue.GetAllAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task getMessageForFutureExpireDate()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var queueMessage = new CB.QueueMessage("data");
            var tomorrow = new DateTime();
            tomorrow.AddDays(1);
            queueMessage.expires = tomorrow;
            List<object> list = new List<object>();
            list.Add(queueMessage);
            var response = await queue.addMessageAsync(list);
            var result = await queue.getMessageAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task addDataIntoQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            await queue.addMessageAsync("sample");
            Assert.IsTrue(true);

        }

        [TestMethod]
        public async Task createAndDeleteQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            await queue.CreateAsync();
            await queue.DeleteAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task addExpireInQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var queueMessage = new CB.QueueMessage("data");
            var tomorrow = new DateTime();
            tomorrow.AddDays(1);
            queueMessage.expires = tomorrow;
            List<object> list = new List<object>();
            list.Add(queueMessage);
            var response = await queue.addMessageAsync(list);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task addCurrentTimeAsExpireDate()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var queueMessage = new CB.QueueMessage("data");
            queueMessage.expires = new DateTime();
            List<object> list = new List<object>();
            list.Add(queueMessage);
            await queue.addMessageAsync(list);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task updateDataIntoTheQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var response = await queue.addMessageAsync("sample");
            if (response.message == "sample")
            {
                response.message = "Hey!";
                List<object> list = new List<object>();
                list.Add(response);
                await queue.updateMessageAsync(list);
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task addMultipleMessageInQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var queueMessage1 = new CB.QueueMessage("sample1");
            var queueMessage2 = new CB.QueueMessage("sample2");
            List<object> list = new List<object>();
            list.Add(queueMessage1);
            list.Add(queueMessage2);
            var response = await queue.addMessageAsync(list);
            //should return array of queuemessage
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task addAndGetDataFromTheQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var message = new CB.QueueMessage("sample");
            List<object> list = new List<object>();
            list.Add(message);
            var response = await queue.addMessageAsync(list);
            if (response.message == "sample")
            {
                var result = await queue.getMessageAsync();
                if (result.message == "sample")
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task shouldPeek()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var message = new CB.QueueMessage("sample");
            List<object> list = new List<object>();
            list.Add(message);
            var response = await queue.addMessageAsync(list);
            if (response.message == "sample")
            {
                var result = await queue.PeekMessageAsync();
                if (result.message == "sample")
                {
                    var result1 = await queue.PeekMessageAsync();
                    if (result.message == "sample")
                    {
                        Assert.IsTrue(true);
                    }
                    Assert.IsFalse(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task shouldGetMessageInFIFO()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var message = new CB.QueueMessage("sample1");
            List<object> list = new List<object>();
            list.Add(message);
            var response = await queue.addMessageAsync(list);
            if (response.message == "sample1")
            {
                message = new CB.QueueMessage("sample2");
                list = new List<object>();
                list.Add(message);
                response = await queue.addMessageAsync(list);
                if (response.message == "sample2")
                {
                    response = await queue.getMessageAsync();
                    if (response.message == "sample1")
                    {
                        response = await queue.getMessageAsync();
                        if (response.message == "sample2")
                        {
                            Assert.IsTrue(true);
                        }
                        Assert.IsFalse(true);
                    }
                    Assert.IsFalse(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task shouldPeekTwoMessageSameTime()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var message = new CB.QueueMessage("sample1");
            List<object> list = new List<object>();
            list.Add(message);
            var response = await queue.addMessageAsync(list);
            if (response.message == "sample1")
            {
                message = new CB.QueueMessage("sample2");
                list = new List<object>();
                list.Add(message);
                response = await queue.addMessageAsync(list);
                if (response.message == "sample2")
                {
                    response = await queue.PeekMessageAsync(2);
                    //should return array of QueueMessage
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task shouldGetTwoMessageSameTime()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var message = new CB.QueueMessage("sample1");
            List<object> list = new List<object>();
            list.Add(message);
            var response = await queue.addMessageAsync(list);
            if (response.message == "sample1")
            {
                message = new CB.QueueMessage("sample2");
                list = new List<object>();
                list.Add(message);
                response = await queue.addMessageAsync(list);
                if (response.message == "sample2")
                {
                    response = await queue.getMessageAsync(2);
                    //should return array of QueueMessage
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task shouldNotGetMessageWithDelay()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var message = new CB.QueueMessage("sample");
            List<object> list = new List<object>();
            list.Add(message);
            var response = await queue.addMessageAsync(list);
            if (response.message == "sample1")
            {
                response = await queue.getMessageAsync();
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task shouldAbleToGetMessageAfterDelay()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var message = new CB.QueueMessage("sample");
            message.delay = 1;
            List<object> list = new List<object>();
            list.Add(message);
            var response = await queue.addMessageAsync(list);
            if (response.message == "sample")
            {
                response = await queue.getMessageAsync();
                if (response.message == "sample")
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task shouldGetMessageWithId()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var message = new CB.QueueMessage("sample");
            message.delay = 1;
            List<object> list = new List<object>();
            list.Add(message);
            var response = await queue.addMessageAsync(list);
            if (response.message == "sample")
            {
                response = await queue.getMessageById(response.id);
                if (response.message == "sample")
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task shouldGetNullForInvalidMessageId()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var message = new CB.QueueMessage("sample");
            message.delay = 1;
            List<object> list = new List<object>();
            list.Add(message);
            var response = await queue.addMessageAsync(list);
            if (response.message == "sample")
            {
                response = await queue.getMessageById("sample");
                if (response == null)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task shouldDeleteMessageWithMessageId()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var message = new CB.QueueMessage("sample");
            message.delay = 1;
            List<object> list = new List<object>();
            list.Add(message);
            var response = await queue.addMessageAsync(list);
            if (response.message == "sample")
            {
                var result = await queue.DeleteMessageAsync(response.id);
                if (result != null && result.id == response.id)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task shouldDeleteMessageByPassingQueueMessageToFunction()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var message = new CB.QueueMessage("sample");
            message.delay = 1;
            List<object> list = new List<object>();
            list.Add(message);
            var response = await queue.addMessageAsync(list);
            if (response.message == "sample")
            {
                var result = await queue.DeleteMessageAsync(response);
                if (result != null && result.id == response.id)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task shouldNotGetMessageAfterItWasDeleted()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var message = new CB.QueueMessage("sample");
            message.delay = 1;
            List<object> list = new List<object>();
            list.Add(message);
            var response = await queue.addMessageAsync(list);
            if (response.message == "sample")
            {
                var result = await queue.DeleteMessageAsync(response);
                if (result != null && result.id == response.id)
                {
                    var obj = queue.getMessageById(response.id);
                    if(obj == null)
                        Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task shouldAddSubscriberToQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var url = "http://sample.sample.com";
            List<object> list = new List<object>();
            list.Add(url);
            var response = await queue.AddSubscriberAsync(list);
            for (int i = 0; i < list.Count; i++)
            {
                if (response.subscribers.Contains(url[i]) == false)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
        }

        [TestMethod]
        public async Task shouldMultipleSubscriberToTheQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var url = "http://sample.sample.com";
            List<object> list = new List<object>();
            list.Add(url);
            var response = await queue.RemoveSubscriberAsync(list);
            for (int i = 0; i < list.Count; i++)
            {
                if (response.subscribers.Contains(url[i]) == false)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
        }

        [TestMethod]
        public async Task shouldRemoveSubscriberFromTheQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var url = "http://sample.sample.com";
            List<object> list = new List<object>();
            list.Add(url);
            var response = await queue.AddSubscriberAsync(list);
            for (int i = 0; i < list.Count; i++)
            {
                if (response.subscribers.Count == 1)
                {
                    var result = await queue.RemoveSubscriberAsync(list);
                    if (response.subscribers.Count == 0)
                    {
                        Assert.IsTrue(true);
                    }
                    Assert.IsFalse(true);
                }
                Assert.IsFalse(true);
            }
        }

        [TestMethod]
        public async Task shouldRemoveMultipleSubscriberFromQueue()
        {

        }

        [TestMethod]
        public async Task shouldNotSubscriberWithInvalidURL()
        {

        }

        [TestMethod]
        public async Task shouldAddAndRemoveSubscriber()
        {

        }

        [TestMethod]
        public async Task shouldDeleteTheQueue()
        {

        }

        [TestMethod]
        public async Task clearTheQueue()
        {

        }

        [TestMethod]
        public async Task shouldGetTheQueue()
        {

        }

        [TestMethod] //1269
        public async Task getAllTest()
        {

        }

        [TestMethod]
        public async Task shouldNotGetTheQueueWhichDoesNotExists()
        {

        }

        [TestMethod]
        public async Task shouldRefreshMessageTimeoutSpecified()
        {

        }

        [TestMethod] //1410
        public async Task updateQueue()
        {

        }
    }
}
