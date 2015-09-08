using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InitAppWithMasterKey()
        {
            CB.Test.Util.Keys.InitWithMasterKey();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void InitAppWithClientKey()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            Assert.IsTrue(true);
        }
    }
}
