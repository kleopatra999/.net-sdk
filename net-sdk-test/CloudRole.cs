using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CB.Test
{
    [TestClass]
    public class CloudRole
    {
        [TestMethod]
        public async Task createRole()
        {
            var roleName = Util.Methods._makeString();
            var role = new CB.CloudRole(roleName);
            var response = await role.SaveAsync();
            if (response != null)
                Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task retrieveRole()
        {
            var roleName = Util.Methods._makeString();
            var role = new CB.CloudRole(roleName);
            var response = await role.SaveAsync();
            if (response.ID == null)
            {
                Assert.IsTrue(true);
            }
            var query = new CB.CloudQuery("Role");
            query.EqualTo("id", response.ID);
            var result = (List<CB.CloudObject>)await query.FindAsync();
            if (result == null)
            {
                throw new CB.Exception.CloudBoostException("Should retrieve the cloud role");
            }
            Assert.IsTrue(true);
        }
    }
}
