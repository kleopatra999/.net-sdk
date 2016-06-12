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
            CB.Test.Util.Keys.InitWithMasterKey();
            CB.CloudNotification.On("sample", new Callback(action));
            Assert.IsTrue(true);
        }

        void action(Object result)
        {
           //do nithign. 
        }

        [TestMethod]
        public void publishDataToChannel()
        {
            CB.Test.Util.Keys.InitWithMasterKey();
            CB.CloudNotification.On("sample", new Callback(anotherAction));
            CB.CloudNotification.Publish("sample", "data");
        }

        void anotherAction(Object result)
        {
            CB.Test.Util.Keys.InitWithMasterKey();
            if (result.ToString() == "data")
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Error wrong data received");
            }
        }

        [TestMethod]
        public void shouldStopListeningChannel()
        {
            CB.Test.Util.Keys.InitWithMasterKey();
            CB.CloudNotification.Off("sample");
            Assert.IsTrue(true);
        }
    }
}
