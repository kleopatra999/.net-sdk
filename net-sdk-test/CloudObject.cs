using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB.Test
{
    [TestClass]
    public class CloudObject
    {
        [TestMethod]
        public void x001_InitAppWithMasterKey()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task saveDataInDateField()
        {
            var obj = new CB.CloudObject("Employee");
            obj.Set("dob", new DateTime());
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task doNotSaveIncorrectEmail()
        {
            var obj = new CB.CloudObject("Custom");
            obj.Set("newColumn", "email");
            try
            {
                obj = await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task saveEmail()
        {
            var obj = new CB.CloudObject("Company");
            obj.Set("Name", "sample");
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task saveDataInCloudObjectWithoutFile()
        {
            var obj = new CB.CloudObject("Custom5");
            obj.Set("dob", new DateTime());
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task saveGeoPoint()
        {
            //TODO: create geopoints then write this test
            var obj = new CB.CloudObject("Custom5");
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
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
                Assert.IsTrue(true);
            }
            
        }

        [TestMethod]
        public async Task doNotSetTheId()
        {
            var obj = new CB.CloudObject("Sample");
            try
            {
                obj.Set("id", "123");
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task Save()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            obj = await obj.SaveAsync();
            if (obj.Get("name") != "sample")
            {
                Assert.IsFalse(true);
            }
            else if (obj.Get("id") == null)
            {
                Assert.IsFalse(true);
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task updateObject()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            obj = await obj.SaveAsync();
            if (obj.Get("name") != "sample")
            {
                Assert.IsFalse(true);
            }
            else if (obj.Get("id") == null)
            {
                Assert.IsFalse(true);
            }

            obj.Set("name", "sample1");
            obj = await obj.SaveAsync();
            if (obj.Get("name") != "sample1")
            {
                Assert.IsFalse(true);
            }
            else if (obj.Get("id") == null)
            {
                Assert.IsFalse(true);
            }

            Assert.IsTrue(true);

        }

        [TestMethod]
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

        [TestMethod]
        public async Task deleteObject()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            obj = await obj.SaveAsync();
            obj = await obj.DeleteAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task requireFieldTest()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            try
            {
                obj = await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task doNotSaveWithWrongDataType()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", 1232);
            try
            {
                obj = await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task duplicationTestInUniqueField()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            obj.Set("unique", "abcd");
            obj = await obj.SaveAsync();
            obj.Set("name", "sample");
            obj.Set("unique", "abcd");
            try
            {
                obj = await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task saveArray()
        {
            string[] text = {"abcd", "abcd"};
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            obj.Set("stringArray", text);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
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
                Assert.IsTrue(true);
            }
        }
        
        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public async Task saveRelationWithRelateFunction()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            var obj1 = new CB.CloudObject("Sample");
            obj1.Set("name", "sample");
            obj1 = await obj1.SaveAsync();
            obj.Relate("sameRelation", "Sample", obj1.Get("id"));
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task keepRelationIntact()
        {
            var obj = new CB.CloudObject("Custom2");
            obj.Set("newColumn2", new CB.CloudObject("Custom3"));
            obj.Set("newColumn7", new CB.CloudObject("student1"));
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task doNotSaveWrongRelation()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            var obj1 = new CB.CloudObject("Sample");
            obj1.Set("name", "sample");
            obj.Set("sameRelation", obj1);
            try
            {
                obj = await obj.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Assert.IsTrue(true);
            }

        }

        [TestMethod]
        public async Task doNotSaveDuplicateRelationInUniqueField()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name", "sample");
            var obj1 = new CB.CloudObject("Sample");
            obj1.Set("name", "sample");
            obj.Set("uniqueRelation", obj1);
            obj = await obj.SaveAsync();
            var obj2 = new CB.CloudObject("Sample");
            obj2.Set("name", "sample");
            obj2.Set("uniqueRelation", obj1);
            
            try
            {
                obj2 = await obj2.SaveAsync();
                Assert.IsFalse(true);
            }
            catch (CB.Exception.CloudBoostException e)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
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

        [TestMethod]
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
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
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

        [TestMethod]
        public async Task saveJsonObjectInColumn()
        {
            Dictionary<string, Object> json = new Dictionary<string,object>();
            json.Add("name", "ranjeet");
            json.Add("location", "uoh");
            json.Add("age", 10);
            var obj = new CB.CloudObject("Custom");
            obj.Set("newColumn6",json);
            obj = await obj.SaveAsync();
            if(obj.Get("name") == "ranjeet" && obj.Get("location") == "uoh" && (int)obj.Get("age") == 10){
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task saveListOfNumbers()
        {
            var obj = new CB.CloudObject("Custom14");
            int[] arr = {1,2,3};
            obj.Set("ListNumber",arr);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task saveListOfGeoPoint()
        {
            //TODO: After GeoPoint Done
            var obj = new CB.CloudObject("Custom14");
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task unsetField()
        {
            var obj1 = new CB.CloudObject("hostel");
            obj1.Set("room",123);
            obj1 = await obj1.SaveAsync();
            if((int)obj1.Get("room") == 123)
            {
                obj1.Unset("room");
                obj1 = await obj1.SaveAsync();
                Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task maintainOrderofSavedRelations()
        {
            var obj1 = new CB.CloudObject("hostel");
            obj1.Set("room",123);
            obj1 = await obj1.SaveAsync();
            if((int)obj1.Get("room") == 123)
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

        [TestMethod]
        public async Task saveRequiredNumberWithZero()
        {
            var obj1 = new CB.CloudObject("Custom18");
            obj1.Set("number", 0);
            obj1 = await obj1.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
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

        //Bulk API Test
        [TestMethod]
        public async Task bulkApiSave()
        {
            var obj = new CB.CloudObject("Student");
            obj.Set("name","Ranjeet");
            var obj1 = new CB.CloudObject("Student");
            obj1.Set("name","ABCD");
            CB.CloudObject[] arr = {obj, obj1};
            List<CB.CloudObject> objects = await CB.CloudObject.SaveAllAsync(arr);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task saveThenDeleteBulkApi()
        {
            var obj = new CB.CloudObject("Student");
            obj.Set("name", "Ranjeet");
            var obj1 = new CB.CloudObject("Student");
            obj1.Set("name", "ABCD");
            CB.CloudObject[] arr = { obj, obj1 };
            List<CB.CloudObject> objects = await CB.CloudObject.SaveAllAsync(arr);
            objects = await CB.CloudObject.DeleteAllAsync(arr);
            Assert.IsTrue(true);
        }

       
        [TestMethod]
        public async Task saveRelationInBulkApi()
        {
            var obj = new CB.CloudObject("Custom2");
            obj.Set("newColumn1", "Course");
            var obj3 = new CB.CloudObject("Custom3");
            obj3.Set("address", "progress");
            obj.Set("newColumn2", obj3);
            CB.CloudObject[] arr = { obj, obj3 };
            List<CB.CloudObject> objects = await CB.CloudObject.SaveAllAsync(arr);
            if (objects[0].Get("id") == ((CB.CloudObject)objects[0].Get("newColumn2")).Get("id"))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(true);
            }
            
        }

        //Version Test
        [TestMethod]
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

        [TestMethod]
        public async Task save()
        {
            var obj = new CB.CloudObject("sample"); 
            obj.Set("name", "sample");
            obj = await obj.SaveAsync();
            if(obj.Get("name") != "sample"){
                Assert.IsFalse(true);
            }
            if(obj.Get("id") == null){
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void getObjectWithVersion()
        {
            //TODO: After Find By Implementation
        }

        [TestMethod]
        public void updateVersionOfSavedObject()
        {
            //TODO: After Find By Implementation
        }

        [TestMethod]
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
            if(user.Get("username") == username && (int)user.Get("_version")>=0){
                Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }

        [TestMethod]
        public async Task createRoleWithVerison()
        {
            var roleName1 = "Admin";
            var role = new CB.CloudRole(roleName1);
            role = (CB.CloudRole)await role.SaveAsync();
            if ((int)role.Get("_version") >= 0)
                Assert.IsTrue(true);

            Assert.IsFalse(true);
        }

        [TestMethod]
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

        [TestMethod]
        public void retrieveSaveUserObject()
        {
            //TODO: After FindById
        }

        [TestMethod]
        public async Task saveRelationWithoutChildObject()
        {
            var obj = new CB.CloudObject("Sample");
            obj.Set("name","ranjeet");
            obj = await obj.SaveAsync();
            if (obj.Get("name") == "ranjeet")
                Assert.IsTrue(true);
            else
                Assert.IsFalse(true);
        }

        //encryption test
        [TestMethod]
        public async Task encryptPassword()
        {
            var email = "ranjeet@abcd.com";
            var obj = new CB.CloudObject("User");
            obj.Set("username", email);
            obj.Set("password","password");
            obj.Set("email",email);
            obj = await obj.SaveAsync();
            if (obj.Get("password") != "password")
                Assert.IsTrue(true);
            else
                Assert.IsFalse(false);
        }

        [TestMethod]
        public async Task encryptEncryptedPassword()
        {
            //TODO: FindById
        }

        //Expire Test
        [TestMethod]
        public async Task saveObjectAfterExpireIsSet()
        {
            var obj = new CB.CloudObject("student1");
            obj.Set("name", "ranjeet");
            obj.Set("age", 10);
            obj = await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task doNotFetchExpiredObjects()
        {
            //TODO: After CloudQuery
        }

        [TestMethod]
        public async Task doNotSearchExpiredObjects()
        {
            //TODO: After CloudSearch
        }

        //File Test
        /*
         * TODO: After CloudFile
        public async Task saveFileInsideObject()
        {
            
        }

        [TestMethod]
        public async Task saveFileWithObjectAndUpdate()
        {
            

        }

        [TestMethod]
        public async Task saveArrayOfFiles()
        {

        }
        */

     }
}
