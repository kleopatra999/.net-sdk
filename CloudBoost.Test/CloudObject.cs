using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CB.Test
{
    [TestFixture]
    public class CloudObject
    {
        [Test]
        public void x001_InitAppWithClientKey()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task saveDataInDateField()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            var obj = new CB.CloudObject("Employee");
            obj.Set("dob", new DateTime());
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task doNotSaveIncorrectEmail()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            var obj = new CB.CloudObject("Custom");
            obj.Set("email", "email");
            try
            {
                await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task saveEmail()
        {
            var obj = new CB.CloudObject("Company");
            obj.Set("Name", "sample");
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task saveDataInCloudObjectWithoutFile()
        {
            var obj = new CB.CloudObject("Custom5");
            obj.Set("dob", new DateTime());
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task saveGeoPoint()
        {
            //TODO: create geopoints then write this test
            var obj = new CB.CloudObject("Custom5");
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task doNotSaveStringIntoDate()
        {
            var obj = new CB.CloudObject("Employee");
            obj.Set("createdAt", "avdv");
            obj.Set("name", "sample");
            try
            {
                obj = await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }
            
        }

        [Test]
        public void doNotSetTheId()
        {
            var obj = new CB.CloudObject("Sample");
            try
            {
                obj.Set("id", "123");
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task Save()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            await obj.SaveAsync();
            if (obj.Get("name").ToString() != "sample")
            {
                Assert.IsFalse(true);
            }
            else if (obj.ID == null)
            {
                Assert.IsFalse(true);
            }

            Assert.IsTrue(true);
        }

        [Test]
        public async Task updateObject()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            await obj.SaveAsync();
            if (obj.Get("name").ToString() != "sample")
            {
                Assert.IsFalse(true);
            }
            else if (obj.ID == null)
            {
                Assert.IsFalse(true);
            }

            obj.Set("name", "sample1");
            await obj.SaveAsync();
            if (obj.Get("name").ToString() != "sample1")
            {
                Assert.IsFalse(true);
            }
            else if (obj.ID == null)
            {
                Assert.IsFalse(true);
            }

            Assert.IsTrue(true);

        }

        [Test]
        public async Task updateSavedObject()
        {
            var obj = new CB.CloudObject("student1");
            var obj1 = new CB.CloudObject("hostel");
            obj1.Set("room", 89787);
            obj1 = await obj1.SaveAsync();
            obj.Set("name", "ranjeet");
            obj = await obj.SaveAsync();
            obj.Set("newColumn", obj1);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task deleteObject()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            obj = await obj.SaveAsync();
            obj = await obj.DeleteAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task requireFieldTest()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            try
            {
                await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task doNotSaveWithWrongDataType()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", 1232);
            try
            {
                await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task duplicationTestInUniqueField()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            obj.Set("unique", "abcd");
            
            try
            {
                await obj.SaveAsync();
                obj.Set("name", "sample");
                obj.Set("unique", "abcd");
                await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task saveArray()
        {
            string[] text = {"abcd", "abcd"};
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            obj.Set("stringArray", text);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task doNotSaveWrongDataTypeInArray()
        {
            int[] text = { 123, 123 };
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            obj.Set("stringArray", text);
            try
            {
                obj = await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }
        }
        
        [Test]
        public async Task saveArrayWithJsonObjects()
        {
            Dictionary<string, Object> jsonObj = new Dictionary<string, object>();
            jsonObj.Add("sample", "sample");
            jsonObj.Add("sample1", "sample1");
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            obj.Set("objectArray", jsonObj);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task saveObjectAsRelation()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            var obj1 = new CB.CloudObject("Sample");
            obj1.Set("name", "sample");
            obj.Set("sameRelation", obj1);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task saveRelationWithRelateFunction()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            var obj1 = new CB.CloudObject("Sample");
            obj1.Set("name", "sample");
            obj1 = await obj1.SaveAsync();
            obj.Relate("sameRelation", "Sample", obj1.Get("id").ToString());
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task keepRelationIntact()
        {
            var obj = new CB.CloudObject("Custom2");
            obj.Set("newColumn2", new CB.CloudObject("Custom3"));
            obj.Set("newColumn7", new CB.CloudObject("student1"));
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task doNotSaveWrongRelation()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            var obj1 = new CB.CloudObject("Sample");
            obj1.Set("name", "sample");
            obj.Set("sameRelation", obj1);
            try
            {
                await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }

        }

        [Test]
        public async Task doNotSaveDuplicateRelationInUniqueField()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            var obj1 = new CB.CloudObject("Sample");
            obj1.Set("name", "sample");
            obj.Set("uniqueRelation", obj1);
            await obj.SaveAsync();
            var obj2 = new CB.CloudObject("Sample");
            obj2.Set("name", "sample");
            obj2.Set("uniqueRelation", obj1);
            
            try
            {
                await obj2.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task shouldModifyListRelationOfSavedObject()
        {
            var obj1 = new CB.CloudObject("Sample");
            obj1.Set("name", "sample");
            var obj2 = new CB.CloudObject("Sample");
            obj2.Set("name", "sample");
            CB.CloudObject[] objects = { obj1, obj2 };
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            obj.Set("relationArray", objects);
            obj = await obj.SaveAsync();
            CB.CloudObject[] relationArray = (CB.CloudObject[])obj.Get("relationArray");
            if (relationArray.Length != 2)
            {
                Assert.IsFalse(true);
            }
            obj.Set("relationArray", relationArray);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task doNotSaveArrayOfDifferentCloudObjects()
        {
            var obj = new CB.CloudObject("Student");
            obj.Set("name", "sample");
            var obj1 = new CB.CloudObject("Sample");
            obj1.Set("name", "sample");
            var obj2 = new CB.CloudObject("Sample");
            obj2.Set("name", "sample");
            CB.CloudObject[] objects = { obj1, obj2 };
            obj.Set("relationArray", objects);
            obj = await obj.SaveAsync();
            try
            {
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task shouldNotDuplicateValuesInListAfterUpdate()
        {
            var obj = new CB.CloudObject("student1");
            obj.Set("age", 5);
            obj.Set("name", "abcd");
            var obj1 = new CB.CloudObject("Custom4");
            CB.CloudObject[] objects = { obj, obj };
            obj1.Set("newColumn7", objects);
            obj1 = await obj1.SaveAsync();
            CB.CloudObject[] arr = (CB.CloudObject[])obj.Get("newColumn7");
            if (arr.Length != 2)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task saveJsonObjectInColumn()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            Dictionary<string, Object> json = new Dictionary<string,object>();
            json.Add("name", "ranjeet");
            json.Add("location", "uoh");
            json.Add("age", 10);
            var obj = new CB.CloudObject("Custom");
            obj.Set("newColumn6",json);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task saveListOfNumbers()
        {
            var obj = new CB.CloudObject("Custom14");
            int[] arr = {1,2,3};
            obj.Set("ListNumber",arr);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task saveListOfGeoPoint()
        {
            //TODO: After GeoPoint Done
            var obj = new CB.CloudObject("Custom14");
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task saveRelation()
        {
            var obj1 = new CB.CloudObject("hostel");
            obj1.Set("room",123);
            obj1 = await obj1.SaveAsync();
            var obj = new CB.CloudObject("student1");

            var obj2 = new CB.CloudObject("hostel", obj1.Get("id").ToString());
            obj.Set("newColumn",obj2);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task unsetField()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            var obj1 = new CB.CloudObject("hostel");
            obj1.Set("room",123);
            obj1 = await obj1.SaveAsync();
            string room = obj1.Get("room").ToString();
            if(room == "123")
            {
                obj1.Unset("room");
                await obj1.SaveAsync();
                Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task maintainOrderofSavedRelations()
        {
            var obj1 = new CB.CloudObject("hostel");
            obj1.Set("room",123);
            obj1 = await obj1.SaveAsync();
            int room = (int)obj1.Get("room");
            if(room == 123)
            {
                obj1.Unset("room");
                obj1 = await obj1.SaveAsync();
                if (obj1.Get("room") == null)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task saveRequiredNumberWithZero()
        {
            var obj1 = new CB.CloudObject("Custom18");
            obj1.Set("number", 0);
            obj1 = await obj1.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task fetchCloudObject()
        {
            var obj1 = new CB.CloudObject("Custom18");
            obj1.Set("number", 0);
            obj1 = await obj1.SaveAsync();
            obj1 = await obj1.FetchAsync();
            if ((int)obj1.Get("number") == 0)
            {
                Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }
      
        //Version Test
        [Test]
        public void setModifiedArray()
        {
            var obj = new CB.CloudObject("sample");
            obj.Set("expires",0);
            obj.Set("name","ranjeet");
            if(((ArrayList)obj.Get("_modifiedColumns")).Count > 0) {
                Assert.IsTrue(true);
            }else{
                Assert.IsFalse(true);
            }
        }

        [Test]
        public async Task save()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            var obj = new CB.CloudObject("Sample"); 
            obj.Set("name", "sample");
            obj = await obj.SaveAsync();
            string name = obj.Get("name").ToString();
            
            if(name != "sample"){
                Assert.IsFalse(true);
            }
            if(obj.ID == null){
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);
        }

        [Test]
        public async Task createNewUserWithVersion()
        {
            var username = "ranjeet";
            var passwd = "abcd";
            var email = "ranjeet@ancv.com";
            var user = new CB.CloudUser();
            user.Set("username", username);
            user.Set("password",passwd);
            user.Set("email", email);
            user = await user.Signup();
            if(user.Get("username").ToString() == username && (int)user.Get("_version")>=0){
                Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task createRoleWithVerison()
        {
            var roleName1 = "Admin";
            var role = new CB.CloudRole(roleName1);
            role = (CB.CloudRole)await role.SaveAsync();
            if ((int)role.Get("_version") >= 0)
                Assert.IsTrue(true);

            Assert.IsFalse(true);
        }

        [Test]
        public async Task storeRelationWithVersion()
        {
            var parent = new CB.CloudObject("Custom4");
            var child = new CB.CloudObject("student1");
            CB.CloudObject[] arr = { child };
            child.Set("name","ranjeet");
            parent.Set("newColumn7", arr);
            parent = await parent.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task saveRelationWithoutChildObject()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name","ranjeet");
            obj = await obj.SaveAsync();
            if (obj.Get("name").ToString() == (string)"ranjeet")
                Assert.IsTrue(true);
            else
                Assert.IsFalse(true);
        }

        //encryption test
        [Test]
        public async Task encryptPassword()
        {
            var email = "ranjeet@abcd.com";
            var obj = new CB.CloudObject("User");
            obj.Set("username", email);
            obj.Set("password","password");
            obj.Set("email",email);
            obj = await obj.SaveAsync();
            string pass = Convert.ToString(obj.Get("password"));
            string val = "password";
            if (pass != (string)val)
                Assert.IsTrue(true);
            else
                Assert.IsFalse(false);
        }

       
        //Expire Test
        [Test]
        public async Task saveObjectAfterExpireIsSet()
        {
            var obj = new CB.CloudObject("student1");
            obj.Set("name", "ranjeet");
            obj.Set("age", 10);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

     }
}
