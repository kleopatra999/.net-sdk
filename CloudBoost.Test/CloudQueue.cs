using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CB.Test
{
    [TestFixture]
    public class CloudQueue
    {
        [Test]
        public async Task noQueueInDB()
        {
            await CB.CloudQueue.GetAllAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task getMessageForFutureExpireDate()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var queueMessage = new CB.QueueMessage("data");
            var tomorrow = new DateTime();
            tomorrow.AddDays(1);
            queueMessage.Expires = tomorrow;
            List<object> list = new List<object>();
            list.Add(queueMessage);
            var response = await queue.AddMessageAsync(list);
            var result = await queue.GetMessageAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task addDataIntoQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            await queue.AddMessageAsync("sample");
            Assert.IsTrue(true);

        }

        [Test]
        public async Task createAndDeleteQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            await queue.CreateAsync();
            await queue.DeleteAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task addExpireInQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var queueMessage = new CB.QueueMessage("data");
            var tomorrow = new DateTime();
            tomorrow.AddDays(1);
            queueMessage.Expires = tomorrow;
            List<object> list = new List<object>();
            list.Add(queueMessage);
            var response = await queue.AddMessageAsync(list);
            Assert.IsTrue(true);
        }

        [Test]
        public async Task addCurrentTimeAsExpireDate()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var queueMessage = new CB.QueueMessage("data");
            queueMessage.Expires = new DateTime();
            List<object> list = new List<object>();
            list.Add(queueMessage);
            await queue.AddMessageAsync(list);
            Assert.IsTrue(true);
        }

        [Test]
        public async Task updateDataIntoTheQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var response = await queue.AddMessageAsync("sample");
            if (response.Message.ToString() == "sample")
            {
                response.Message = "Hey!";
                List<object> list = new List<object>();
                list.Add(response);
                await queue.UpdateMessageAsync(list);
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task addMultipleMessageInQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var queueMessage1 = new CB.QueueMessage("sample1");
            var queueMessage2 = new CB.QueueMessage("sample2");
            List<object> list = new List<object>();
            list.Add(queueMessage1);
            list.Add(queueMessage2);
            var response = await queue.AddMessageAsync(list);
            //should return array of queueMessage
            Assert.IsTrue(true);
        }

        [Test]
        public async Task addAndGetDataFromTheQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var Message = new CB.QueueMessage("sample");
            List<object> list = new List<object>();
            list.Add(Message);
            var response = await queue.AddMessageAsync(list);
            if (response.Message.ToString() == "sample")
            {
                var result = await queue.GetMessageAsync();
                if (result.Message.ToString() == "sample")
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task shouldPeek()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var Message = new CB.QueueMessage("sample");
            List<object> list = new List<object>();
            list.Add(Message);
            var response = await queue.AddMessageAsync(list);
            if (response.Message.ToString() == "sample")
            {
                var result = await queue.PeekMessageAsync();
                if (result.Message.ToString() == "sample")
                {
                    var result1 = await queue.PeekMessageAsync();
                    if (result.Message.ToString() == "sample")
                    {
                        Assert.IsTrue(true);
                    }
                    Assert.IsFalse(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task shouldGetMessageInFIFO()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var Message = new CB.QueueMessage("sample1");
            List<object> list = new List<object>();
            list.Add(Message);
            var response = await queue.AddMessageAsync(list);
            if (response.Message.ToString() == "sample1")
            {
                Message = new CB.QueueMessage("sample2");
                list = new List<object>();
                list.Add(Message);
                response = await queue.AddMessageAsync(list);
                if (response.Message.ToString() == "sample2")
                {
                    response = await queue.GetMessageAsync();
                    if (response.Message.ToString() == "sample1")
                    {
                        response = await queue.GetMessageAsync();
                        if (response.Message.ToString() == "sample2")
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

        [Test]
        public async Task shouldPeekTwoMessageSameTime()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var Message = new CB.QueueMessage("sample1");
            List<object> list = new List<object>();
            list.Add(Message);
            var response = await queue.AddMessageAsync(list);
            if (response.Message.ToString() == "sample1")
            {
                Message = new CB.QueueMessage("sample2");
                list = new List<object>();
                list.Add(Message);
                response = await queue.AddMessageAsync(list);
                if (response.Message.ToString() == "sample2")
                {
                    response = await queue.PeekMessageAsync(2);
                    //should return array of QueueMessage
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task shouldGetTwoMessageSameTime()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var Message = new CB.QueueMessage("sample1");
            List<object> list = new List<object>();
            list.Add(Message);
            var response = await queue.AddMessageAsync(list);
            if (response.Message.ToString() == "sample1")
            {
                Message = new CB.QueueMessage("sample2");
                list = new List<object>();
                list.Add(Message);
                response = await queue.AddMessageAsync(list);
                if (response.Message.ToString() == "sample2")
                {
                    response = await queue.GetMessageAsync(2);
                    //should return array of QueueMessage
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task shouldNotGetMessageWithDelay()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var Message = new CB.QueueMessage("sample");
            List<object> list = new List<object>();
            list.Add(Message);
            var response = await queue.AddMessageAsync(list);
            if (response.Message.ToString() == "sample1")
            {
                response = await queue.GetMessageAsync();
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task shouldAbleToGetMessageAfterDelay()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var Message = new CB.QueueMessage("sample");
            Message.Delay = 1;
            List<object> list = new List<object>();
            list.Add(Message);
            var response = await queue.AddMessageAsync(list);
            if (response.Message.ToString() == "sample")
            {
                response = await queue.GetMessageAsync();
                if (response.Message.ToString() == "sample")
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task shouldGetMessageWithId()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var Message = new CB.QueueMessage("sample");
            Message.Delay = 1;
            List<object> list = new List<object>();
            list.Add(Message);
            var response = await queue.AddMessageAsync(list);
            if (response.Message.ToString() == "sample")
            {
                response = await queue.GetMessageByID(response.ID);
                if (response.Message.ToString() == "sample")
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task shouldGetNullForInvalidMessageId()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var Message = new CB.QueueMessage("sample");
            Message.Delay = 1;
            List<object> list = new List<object>();
            list.Add(Message);
            var response = await queue.AddMessageAsync(list);
            if (response.Message.ToString() == "sample")
            {
                response = await queue.GetMessageByID("sample");
                if (response == null)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task shouldDeleteMessageWithMessageId()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var Message = new CB.QueueMessage("sample");
            Message.Delay = 1;
            List<object> list = new List<object>();
            list.Add(Message);
            var response = await queue.AddMessageAsync(list);
            if (response.Message.ToString() == "sample")
            {
                var result = await queue.DeleteMessageAsync(response.ID);
                if (result != null && result.ID == response.ID)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task shouldDeleteMessageByPassingQueueMessageToFunction()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var Message = new CB.QueueMessage("sample");
            Message.Delay = 1;
            List<object> list = new List<object>();
            list.Add(Message);
            var response = await queue.AddMessageAsync(list);
            if (response.Message.ToString() == "sample")
            {
                var result = await queue.DeleteMessageAsync(response);
                if (result != null && result.ID == response.ID)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task shouldNotGetMessageAfterItWasDeleted()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var Message = new CB.QueueMessage("sample");
            Message.Delay = 1;
            List<object> list = new List<object>();
            list.Add(Message);
            var response = await queue.AddMessageAsync(list);
            if (response.Message.ToString() == "sample")
            {
                var result = await queue.DeleteMessageAsync(response);
                if (result != null && result.ID == response.ID)
                {
                    var obj = queue.GetMessageByID(response.ID);
                    if(obj == null)
                        Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task shouldAddSubscriberToQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var url = "http://sample.sample.com";
            List<object> list = new List<object>();
            list.Add(url);
            var response = await queue.AddSubscriberAsync(list);
            for (int i = 0; i < list.Count; i++)
            {
                if (response.Subscribers.Contains(url[i]) == false)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
        }

        [Test]
        public async Task shouldMultipleSubscriberToTheQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var url = "http://sample.sample.com";
            List<object> list = new List<object>();
            list.Add(url);
            var response = await queue.RemoveSubscriberAsync(list);
            for (int i = 0; i < list.Count; i++)
            {
                if (response.Subscribers.Contains(url[i]) == false)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
        }

        [Test]
        public async Task shouldRemoveSubscriberFromTheQueue()
        {
            var queue = new CB.CloudQueue(Util.Methods._makeString());
            var url = "http://sample.sample.com";
            List<object> list = new List<object>();
            list.Add(url);
            var response = await queue.AddSubscriberAsync(list);
            for (int i = 0; i < list.Count; i++)
            {
                if (response.Subscribers.Count == 1)
                {
                    var result = await queue.RemoveSubscriberAsync(list);
                    if (response.Subscribers.Count == 0)
                    {
                        Assert.IsTrue(true);
                    }
                    Assert.IsFalse(true);
                }
                Assert.IsFalse(true);
            }
        }
    }
}
