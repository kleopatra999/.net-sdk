//using System;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System.Linq;
//using NUnit.Framework;

//namespace CB.Test
//{
//    [TestFixture]
//    public class CloudRole
//    {
//        [Test]
//        public async Task CreateRole()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var roleName = Util.Methods.MakeString();
//            var role = new CB.CloudRole(roleName);
//            var response = await role.SaveAsync();
//            if (response != null)
//                Assert.IsTrue(true);
//        }

//        [Test]
//        public async Task RetrieveRole()
//        {
//            CB.Test.Util.Keys.InitWithMasterKey();
//            var roleName = Util.Methods.MakeString();
//            var role = new CB.CloudRole(roleName);
//            var response = await role.SaveAsync();
//            if (response.ID == null)
//            {
//                Assert.IsTrue(true);
//            }
//            var query = new CB.CloudQuery("Role");
//            query.EqualTo("id", response.ID);
//            var result = (List<CB.CloudObject>)await query.FindAsync();
//            if (result == null)
//            {
//                Assert.Fail("Should retrieve the cloud role");
//            }
//            Assert.IsTrue(true);
//        }
//    }
//}
