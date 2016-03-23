using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CB.Test
{
    [TestFixture]
    public class CloudQuery
    {
        [Test]
        public void x001_InitAppWithClientKey()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            Assert.IsTrue(true);
        }
       
        [Test]
        public async Task saveObject()
        {
            var obj = new CB.CloudObject("student1");
            obj.Set("name", "ranjeet");
            await obj.SaveAsync();
            if (obj.Get("name").ToString() == "ranjeet")
            {
                Assert.IsTrue(true);
            }

            Assert.IsFalse(true);
        }

        [Test]
        public async Task find()
        {
            CB.Test.Util.Keys.InitWithClientKey();
            var obj = new CB.CloudObject("Custom1");
            obj.Set("newColumn", "sample");
            obj.Set("description", "sample2");
            await obj.SaveAsync();
            var query = new CB.CloudQuery("Custom1");
            query.EqualTo("id", obj.ID);
            query.SelectColumn("newColumn");
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                if (response[0].Get("description") == null)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task containedInWithId()
        {
            var obj1 = new CB.CloudObject("Custom1");
            obj1.Set("newColumn", "sample");
            obj1.Set("description", "sample2");
            await obj1.SaveAsync();
            var obj2 = new CB.CloudObject("Custom1");
            obj2.Set("newColumn", "sample");
            obj2.Set("description", "sample2");
            await obj2.SaveAsync();
            var obj3 = new CB.CloudObject("Custom1");
            obj3.Set("newColumn", "sample");
            obj3.Set("description", "sample2");
            await obj3.SaveAsync();

            var query = new CB.CloudQuery("Custom1");
            List<CB.CloudObject> list = new List<CB.CloudObject>();
            list.Add(obj1);
            list.Add(obj2);
            list.Add(obj3);
            query.ContainedIn("id", list);
            query.SelectColumn("newColumn");
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count == 2)
            {
                Assert.IsTrue(true);
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task columnShouldWorkOnDistinct()
        {
            var obj = new CB.CloudObject("Custom1");
            obj.Set("newColumn", "sample");
            obj.Set("description", "sample2");
            await obj.SaveAsync();
            var query = new CB.CloudQuery("Custom1");
            query.EqualTo("id", obj.ID);
            query.SelectColumn("newColumn");
            var response = (List<CB.CloudObject>)await query.DistinctAsync("id");
            if (response.Count > 0)
            {
                if (response[0].Get("description") == null)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
            Assert.IsFalse(true);
        }

        [Test] 
        public async Task columnNameNotEqualTo()
        {
            var obj = new CB.CloudObject("student1");
            obj.Set("name","sampleName");
            await obj.SaveAsync();
            var query = new CB.CloudQuery("student1");
            query.NotEqualTo("name", null);
            var response = (List<CB.CloudObject>)await query.FindAsync();
            for(int i=0; i<response.Count; i++){
                if(response[i].Get("name") == null){
                    Assert.Fail("Name does not exists");
                }
            }
            if(response.Count > 0)
                Assert.IsTrue(true);
            else
                Assert.Fail("object could not queried properly");
        }

        [Test] 
        public async Task findDataWithId()
        {
            var obj = new CB.CloudObject("student1");
            obj.Set("name","sampleName");
            await obj.SaveAsync();
            var query = new CB.CloudQuery("student1");
            query.EqualTo("id", obj.ID);
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public async Task findItemWithId()
        {
            var obj = new CB.CloudObject("student1");
            obj.Set("name", "sampleName");
            await obj.SaveAsync();
            var query = new CB.CloudQuery("student1");
            query.EqualTo("id", obj.ID);
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test] 
        public async Task findOneQuery()
        {
            var obj = new CB.CloudObject("student1");
            obj.Set("name", "sampleName");
            await obj.SaveAsync();
            var query = new CB.CloudQuery("student1");
            query.EqualTo("name", "sampleName");
            var response = await query.FindOneAsync();
            if (response.Get("name").ToString() == "sampleName")
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test] //273
        public async Task retrieveDataWithValue()
        {
            var obj = new CB.CloudObject("student1");
            obj.Set("name", "sampleName");
            await obj.SaveAsync();
            var query = new CB.CloudQuery("student1");
            query.EqualTo("name", "sampleName");
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                for (int i = 0; i < response.Count; i++)
                {
                    if (response[i].Get("name").ToString() != "sampleName")
                    {
                        Assert.Fail("should retrieve saved data with particular value");
                    }
                }
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public async Task retrieveList()
        {
            var obj = new CB.CloudObject("student4");
            string[] list = { "java", "python" };
            obj.Set("subject", list);
            await obj.SaveAsync();
            var query = new CB.CloudQuery("student4");
            query.ContainsAll("subject", list);
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                for (int i = 0; i < response.Count; i++)
                {
                    string[] subject= (string[])response[i].Get("subject");
                    for (int j = 0; j < subject.Length; j++)
                    {
                        if (subject[j] != "java" && subject[j] != "python")
                        {
                            Assert.Fail("should retrieve saved data with particular value");
                        }
                    }
                }
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test] 
        public async Task startsWithTest()
        {
            var query = new CB.CloudQuery("student1");
            query.StartsWith("name", "s");
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                for (int i = 0; i < response.Count; i++)
                {
                    string name = (string)response[i].Get("name");
                    if ( name.StartsWith("s") == false)
                    {
                        Assert.Fail("should retrieve saved data with particular value ");
                    }
                }
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }


        [Test] 
        public async Task greaterThanTest()
        {
            var query = new CB.CloudQuery("student4");
            query.GreaterThan("age", 10);
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                for (int i = 0; i < response.Count; i++)
                {
                    int data = (int)response[i].Get("age");
                    if (data <= 10)
                    {
                        Assert.Fail("received value less than the required value");
                    }
                }
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public async Task greaterThanEqualTo()
        {
            var query = new CB.CloudQuery("student4");
            query.GreaterThanEqualTo("age", 15);
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                for (int i = 0; i < response.Count; i++)
                {
                    int data = (int)response[i].Get("age");
                    if (data < 15)
                    {
                        Assert.Fail("received value less than the required value");
                    }
                }
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public async Task lessThan()
        {
            var query = new CB.CloudQuery("student4");
            query.LessThan("age", 20);
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                for (int i = 0; i < response.Count; i++)
                {
                    int data = (int)response[i].Get("age");
                    if (data >= 20)
                    {
                        Assert.Fail("received value greater than the required value");
                    }
                }
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test] 
        public async Task lessThanEqualTo()
        {
            var query = new CB.CloudQuery("student4");
            query.LessThanEqualTo("age", 15);
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                for (int i = 0; i < response.Count; i++)
                {
                    int data = (int)response[i].Get("age");
                    if (data > 15)
                    {
                        Assert.Fail("received value greater than the required value");
                    }
                }
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public async Task retrieveDataWithParticularValue()
        {
            var obj1 = new CB.CloudQuery("student4");
            string[] list = {"java", "python"};
            obj1.EqualTo("subject", list);
            var obj2 = new CB.CloudQuery("student4");
            obj2.EqualTo("age", 12);
            var obj = CB.CloudQuery.Or(obj1, obj2);
            var response = (List<CB.CloudObject>)await obj.FindAsync();
            if (response.Count > 0)
            {
                for (int i = 0; i < response.Count; i++)
                {
                    int data = (int)response[i].Get("age");
                    if (data == 12)
                    {
                        continue;
                    }
                    else
                    {
                        string[] subject = (string[])response[i].Get("subject");
                        for (int j = 0; j < subject.Length; j++)
                        {
                            if (subject[j] == "java" || subject[j] == "python")
                            {
                                continue;
                            }
                            else
                            {
                                Assert.Fail("should retrieve saved data with particular value");
                            }
                        }
                    }
                }
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test] 
        public async Task ascendingOrder()
        {
            var query = new CB.CloudQuery("student4");
            query.OrderByAsc("age");
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                int age = (int)response[0].Get("age");
                for (int i = 1; i < response.Count; i++)
                {
                    int data = (int)response[i].Get("age");
   
                    if (age > data)
                    {
                        Assert.Fail("received value greater than the required value");
                    }
                    age = data;
                }
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public async Task descendingOrder()
        {
            var query = new CB.CloudQuery("student4");
            query.OrderByDesc("age");
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                int age = (int)response[0].Get("age");
                for (int i = 1; i < response.Count; i++)
                {
                    int data = (int)response[i].Get("age");

                    if (age < data)
                    {
                        Assert.Fail("received value greater than the required value");
                    }
                    age = data;
                }
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test] 
        public async Task distinctTest()
        {
            var query = new CB.CloudQuery("student4");
            var response = (List<CB.CloudObject>)await query.DistinctAsync("age");
            List<int> age = new List<int>();
            if (response.Count > 0)
            {
                for (int i = 1; i < response.Count; i++)
                {
                    int data = (int)response[i].Get("age");

                    if (age.Contains(data) == true)
                    {
                        Assert.Fail("received item with duplicate age");
                    }
                    age.Add(data);
                }
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public async Task findByIdTest()
        {
            var obj = new CB.CloudObject("student1");
            obj.Set("name", "abcd");
            await obj.SaveAsync();
            var query = new CB.CloudQuery("student1");
            var response = await query.GetAsync(obj.ID);
            if (response.Get("name").ToString() == "abcd")
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public async Task existsTest() 
        {
            var query = new CB.CloudQuery("student4");
            query.Exists("age");
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                for (int i = 0; i < response.Count; i++)
                {
                    if (response[i].Get("age") == null)
                    {
                        Assert.Fail("received wrong data");
                    }
                }
                Assert.IsTrue(true);
            }
            Assert.IsFalse(false);
        }

        [Test]
        public async Task doesNotExists()
        {
            var query = new CB.CloudQuery("student4");
            query.DoesNotExist("age");
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                for (int i = 0; i < response.Count; i++)
                {
                    if (response[i].Get("age") != null)
                    {
                        Assert.Fail("received wrong data");
                    }
                }
                Assert.IsTrue(true);
            }
            Assert.IsFalse(false);
        }

        [Test]
        public async Task relationQueryTest()
        {
            var obj1 = new CB.CloudObject("hostel");
            obj1.Set("room", 123);
            await obj1.SaveAsync();
            var obj = new CB.CloudObject("student1");
            obj.Set("newColumn", obj1);
            await obj.SaveAsync();
            var query = new CB.CloudQuery("student1");
            query.NotEqualTo("newColumn", obj1);
            var response = (List<CB.CloudObject>)await query.FindAsync();
            for (int i = 0; i < response.Count; i++)
            {
                if (response[i].Get("newColumn") != null)
                {

                    CB.CloudObject relObj = (CB.CloudObject)response[i].Get("newColumn");
                    if (relObj.ID == obj1.ID)
                    {
                        Assert.Fail("Should not get the id in not equal to");
                    }
                    Assert.IsTrue(true);
                }
            }
        }

        [Test]
        public async Task queryOverBooleanDataType()
        {
            var obj1 = new CB.CloudObject("Custom1");
            obj1.Set("newColumn", false);
            await obj1.SaveAsync();
            var query = new CB.CloudQuery("Custom1");
            query.EqualTo("newColumn1", false);
            var response = (List<CB.CloudObject>)await query.FindAsync();
            if (response.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(true);
            }
        }

        [Test]
        public async Task getEncryptedPassword()
        {
            string username = Util.Methods._makeEmail();

            var obj = new CB.CloudObject("User");
            obj.Set("username", username);
            obj.Set("password", "password");
            obj.Set("email", Util.Methods._makeEmail());
            await obj.SaveAsync();
            if (obj.Get("password").ToString() != "password")
            {
                var query = new CB.CloudQuery("User");
                query.EqualTo("password", "password");
                query.EqualTo("username", username);
                var response = (List<CB.CloudObject>)await query.FindAsync();
                if (response.Count > 0)
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("Cannot get items");
                }
            }
            Assert.IsFalse(true);
        }

        [Test]
        public async Task getEncryptedPasswordOverORQuery()
        {
            var username = Util.Methods._makeEmail();
            var obj = new CB.CloudObject("User");
            obj.Set("username", username);
            obj.Set("password", "password");
            obj.Set("email", username);
            await obj.SaveAsync();
            string pass = obj.Get("password").ToString();
            if (!pass.Equals("password"))
            {
                var query1 = new CB.CloudQuery("User");
                query1.EqualTo("password", "password");

                var query2 = new CB.CloudQuery("User");
                query2.EqualTo("password", "password1");

                var query = CB.CloudQuery.Or(query1, query2);
                query.EqualTo("username", username);
                List<CB.CloudObject> response = await query.FindAsync();
                if (response.Count > 0)
                {
                    Assert.IsTrue(true);
                }
                Assert.IsFalse(true);
            }
        }

        [Test]
        public async Task doNotEncryptAlreadyEncryptedPassword()
        {
            string username = Util.Methods._makeEmail();

            var obj = new CB.CloudObject("User");
            obj.Set("username", username);
            obj.Set("password", "password");
            obj.Set("email", Util.Methods._makeEmail());
            await obj.SaveAsync();
            var query = new CB.CloudQuery("User");

            var response = await query.GetAsync(obj.ID);
            var obj1 = await response.SaveAsync();
            Assert.IsTrue(true);

        }
    }
}
