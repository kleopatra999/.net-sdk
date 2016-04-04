using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CB.Test
{
    [TestFixture]
    class CloudPush
    {
        [Test]
        public void x001_InitAppWithClientKey()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task pushNoticeTest()
        {
            var pushObj = new CB.CloudPush();
            await pushObj.InitAsync();
        }
    }
}
