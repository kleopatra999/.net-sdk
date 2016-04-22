using System;
using NUnit.Framework;

namespace CB.Test
{
    [TestFixture]
    public class CloudNotification
    {
        [Test]
        public void subscribeToChannel()
        {
            CB.CloudNotification.On("sample", new Callback(action));

        }

        void action(Object result)
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void publishDataToChannel()
        {
            CB.CloudNotification.On("sample", new Callback(anotherAction));
        }

        void anotherAction(Object result)
        {
            if (result.ToString() == "data")
            {
                CB.CloudNotification.Publsh("sample", "data");
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Error wrong data received");
            }
        }

        [Test]
        public void shouldStopListeningChannel()
        {
            CB.CloudNotification.Off("sample");
            Assert.IsTrue(true);
        }
    }
}
