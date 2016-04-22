using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CB.Test
{
    [TestFixture]
    public class CloudUser
    {
        protected string username = Util.Methods._makeString();
        protected string password = "abcd";
        [Test]
        public async Task createNewUser()
        {
            var obj = new CB.CloudUser();
            obj.Set("username", username);
            obj.Set("password", password);
            obj.Set("email", Util.Methods._makeEmail());
            await obj.Signup();
            if (obj.Username == username)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Error Creating User");
            }
        }

        [Test]
        public async Task shouldLogoutUser()
        {
            await CB.CloudUser.Current.Logout();
        }

        [Test]
        public async Task createUserAndGetVersion()
        {
            username = Util.Methods._makeString();
            var obj = new CB.CloudUser();
            obj.Set("username", username);
            obj.Set("password", password);
            obj.Set("email", Util.Methods._makeEmail());
            await obj.Signup();
            if (obj.Username == username && (int)obj.Get("_version")>=0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Error Creating User");
            }
        }

        [Test]
        public async Task queryOnUser()
        {
            username = Util.Methods._makeString();
            var obj = new CB.CloudUser();
            obj.Set("username", username);
            obj.Set("password", password);
            obj.Set("email", Util.Methods._makeEmail());
            await obj.Signup();
            if (obj.Username == username && (int)obj.Get("_version") >= 0)
            {
                var query = new CB.CloudQuery("User");
                var response = await query.GetAsync(obj.ID);
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Error Creating User");
            }
        }
        [Test]
        public async Task logoutUser()
        {
            await CB.CloudUser.Current.Logout();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task loginUser()
        {
            var obj = new CB.CloudUser();
            obj.Set("username", username);
            obj.Set("password", password);
            await obj.Login();
            if (obj.Username == username)
            {
               Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("user login error");
            }
        }

        [Test]
        public async Task assignRoleToUser()
        {
            var roleName = Util.Methods._makeString();
            var role = new CB.CloudRole(roleName);
            role.Set("name",roleName);
            var obj = new CB.CloudUser();
            obj.Set("username", username);
            obj.Set("password", password);
            await obj.Login();
            await role.SaveAsync();
            await obj.AddToRole(role);
            Assert.IsTrue(true);
        }

        [Test]
        public async Task removeRoleAssignRoleToUser()
        {
            var obj = new CB.CloudUser();
            var roleName = Util.Methods._makeString();
            var role = new CB.CloudRole(roleName);
            role.Set("name", roleName);
            obj.Set("username", username);
            obj.Set("password", password);
            await obj.Login();
            await role.SaveAsync();
            await obj.AddToRole(role);
            await CB.CloudUser.Current.RemoveFromRole(role);
            Assert.IsTrue(true);
        }

        [Test]
        public async Task shouldEncryptUserPassword()
        {
            var obj = new CB.CloudUser();
            obj.Set("username", username);
            obj.Set("password", password);
            obj.Set("email", Util.Methods._makeEmail());
            await obj.SaveAsync();
            if (obj.Password == username)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Error Creating User");
            }
        }

        [Test]
        public async Task createNewUserSave()
        {
            var obj = new CB.CloudUser();
            obj.Set("username", username);
            obj.Set("password", password);
            obj.Set("email", Util.Methods._makeEmail());
            await obj.SaveAsync();
            var query = new CB.CloudQuery("User");
            var response = await query.GetAsync(obj.ID);
            if (response != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Unable to retrieve User");
            }
        }

    }
}
