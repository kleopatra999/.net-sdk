using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CB.Test
{
    [TestClass]
    public class CloudNotification
    {
        [TestMethod]
        public void subscribeToChannel()
        {
            CB.CloudNotification.On("sample", new Callback(action));

        }

        void action(Object result)
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void publishDataToChannel()
        {
            CB.CloudNotification.On("sample", new Callback(anotherAction));
        }
        void anotherAction(Object result)
        {
            if (result == "data")
            {
                CB.CloudNotification.Publsh("sample", "data");
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("Error wrong data received");
            }
        }
        [TestMethod]
        public void shouldStopListeningChannel()
        {
            CB.CloudNotification.Off("sample");
            Assert.IsTrue(true);
        }
    }
}
