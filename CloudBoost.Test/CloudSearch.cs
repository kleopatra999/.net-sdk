using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CB.Test
{
    [TestFixture]
    public class CloudSearch
    {
        [Test]
        public void x001_InitAppWithMasterKey()
        {
            var tableName = CB.Test.Util.Methods._makeString();

            CB.Test.Util.Keys.InitWithMasterKey();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task getDataFromServerNearFunction()
        {
            var custom = new CB.CloudTable("CustomGeoPoint");
            var newColumn7 = new CB.Column("location");
            newColumn7.DataType= CB.DataType.GeoPoint.ToString();
            custom.AddColumn(newColumn7);
            var response = await custom.SaveAsync();
            var loc = new CB.CloudGeoPoint(17.7,80.0);
            var obj = new CB.CloudObject("CustomGeoPoint");
            obj.Set("location", loc);
            await obj.SaveAsync();
            var search = new CB.CloudSearch("CustomGeoPoint");
            
            search.SearchFilter = new CB.SearchFilter();
            search.SearchFilter.Near("location", loc, 1);
            var list = (List<CB.CloudObject>)await search.Search();
            if(list.Count > 0)
            {
                Assert.IsTrue(true);
            }else
            {
                Assert.Fail("should have retrieved data");
            }
        }

        [Test]
        public async Task EqualToWithCloudSearchOverCloudObject()
        {
            var custom = new CB.CloudTable("CustomRelation");
            var newColumn1 = new CB.Column("newColumn7");
            newColumn1.DataType = CB.DataType.Relation.ToString();
            custom.AddColumn(newColumn1);
            await custom.SaveAsync();
            var loc = new CB.CloudGeoPoint(17.7, 80.0);
            var obj = new CB.CloudObject("CustomRelation");
            var obj1 = new CB.CloudObject("student1");
            obj1.Set("name", "Ranjeet");
            obj.Set("newColumn7", obj1);
            await obj.SaveAsync();
            var search = new CB.CloudSearch("CustomRelation");
            search.SearchFilter = new CB.SearchFilter();
            search.SearchFilter.EqualTo("newColumn7", obj.Get("newColumn7"));
            var list = (List<CB.CloudObject>)await search.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("should have retrieved data");
            }
        }


        [Test]
        public async Task indexObject()
        {
            var obj = new CB.CloudObject("Custom1");
            obj.Set("description", "wi-fi");
            await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task searchIndexedObject()
        {
            var cs = new CB.CloudSearch("Custom1");
            cs.SearchQuery = new CB.SearchQuery();
            cs.SearchQuery.SearchOn("description", "wi-fi", null, null, null, null);
            var list = (List<CB.CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task indexTestData()
        {
            var obj = new CB.CloudObject("Student");
            obj.Set("description", "This is nawaz");
            obj.Set("age", 19);
            obj.Set("name", "Nawaz Dhandala");
            obj.Set("class", "Java");
            await obj.SaveAsync();
  
            obj.Set("description", "This is ravi");
            obj.Set("age", 40);
            obj.Set("class", "C#");
            await obj.SaveAsync();

            Assert.IsTrue(true);
        }

        [Test]
        public async Task searchObjectForGivenValue()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();

            cs.SearchFilter.EqualTo("age", 19);
            var list = (List<CB.CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public async Task searchObjectWithPhrase()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchQuery = new CB.SearchQuery();

            cs.SearchQuery.Phrase("name", "Gautam Singh", null, null);
            var list = (List<CB.CloudObject>)await cs.Search();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task searchObjectWithWhileCard()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchQuery = new CB.SearchQuery();

            cs.SearchQuery.WildCard("name", "G*", null);
            var list = (List<CB.CloudObject>)await cs.Search();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task searchForObjectWithStartWith()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchQuery = new CB.SearchQuery();

            cs.SearchQuery.StartsWith("name", "G", null);
            var list = (List<CB.CloudObject>)await cs.Search();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task searchObjectWithMostColumns()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchQuery = new CB.SearchQuery();
            string[] column = { "name", "description" };
            cs.SearchQuery.MostColumns(column, "G", null, null, null, null);
            var list = (List<CB.CloudObject>)await cs.Search();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task searchForObjectWithBestColumns()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchQuery= new CB.SearchQuery();
            string[] column = { "name", "description" };
            cs.SearchQuery.BestColumns(column, "G", null, null, null, null);
            var list = (List<CB.CloudObject>)await cs.Search();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task shouldSearchWithNotEqualToValue()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();
            cs.SearchFilter.NotEqualTo("age", 19);
            var list = (List<CB.CloudObject>)await cs.Search();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task setLimitTest()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();
            cs.SearchFilter.NotEqualTo("age", 19);
            cs.Limit = 0;
            var list = (List<CB.CloudObject>)await cs.Search();
            if (list.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("should limit the number of results");
            }

        }

        [Test]
        public async Task skipTest()
        {
            var obj = new CB.CloudObject("Student");
            obj.Set("age",21);
            await obj.SaveAsync();
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();
            cs.SearchFilter.NotEqualTo("age", 19);
            cs.Skip = 999999;
            var list = (List<CB.CloudObject>)await cs.Search();
            if (list.Count == 0)
            {
                cs = new CB.CloudSearch("Student");
                cs.SearchFilter = new CB.SearchFilter();
                cs.SearchFilter.NotEqualTo("age", 19);
                cs.Skip = 1;
                list = (List<CB.CloudObject>)await cs.Search();
                if (list.Count > 0)
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("");
                }
            }
            else
            {
                Assert.Fail("");
            }
        }

        [Test]
        public async Task sortAscendingOrder()
        {
            var cs = new CB.CloudSearch("Student");
            cs.OrderByAsc("age");
            var list = (List<CB.CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("");
            }
        }

        [Test]
        public async Task sortInDescendingOrder()
        {
            var cs = new CB.CloudSearch("Student");
            cs.OrderByDesc("age");
            var list = (List<CB.CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("");
            }
        }

        [Test]
        public async Task columnExistsTest()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();
            cs.SearchFilter.Exists("name");
            var list = (List<CB.CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("");
            }
        }

        [Test]
        public async Task searchRecordsWhichDoNotHaveCertainColumnTest()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();
            cs.SearchFilter.DoesNotExists("expire");
            var list = (List<CB.CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("");
            }
        }

        [Test]
        public async Task recordsWithinCertainRange()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();
           
            cs.SearchFilter.GreaterThan("age",19);
            cs.SearchFilter.LessThan("age",50);
            var list = (List<CB.CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("");
            }
        }

        [Test]
        public async Task OrBetweenTables()
        {
            var obj = new CB.CloudObject("Student");
            obj.Set("name", "RAVI");

            var obj1 = new CB.CloudObject("hostel");
            obj1.Set("room", 509);

            await obj.SaveAsync();
            await obj1.SaveAsync();

            ArrayList tableNames = new ArrayList();
            tableNames.Add("Student");
            tableNames.Add("hostel");

            var sq = new CB.SearchQuery();
            sq.SearchOn("name", "ravi", null, null, null, null);

            var sq1 = new CB.SearchQuery();
            sq1.SearchOn("room",509, null, null, null, null);

            var cs = new CB.CloudSearch(tableNames);
            cs.SearchQuery = new SearchQuery();
            cs.SearchQuery.Or(sq);
            cs.SearchQuery.Or(sq1);

            var list = (List<CB.CloudObject>)await cs.Search();
            var table = tableNames.ToArray();
            for (int i = 0; i < list.Count; i++)
            {

                if (list[i].TableName != null)
                {
                    var name = (string)list[i].TableName;
                     
                    table = table.Where(val => val.ToString() != name).ToArray();
                }
            }

            if (table.Length == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Search on both tables with OR failed.");
            }
        }

        [Test]
        public async Task runOperatorQueries()
        {
            var obj = new CB.CloudObject("Student");
            obj.Set("name", "RAVI");
            await obj.SaveAsync();

            ArrayList tableNames = new ArrayList();
            tableNames.Add("Student");
            var cs = new CB.CloudSearch(tableNames);
            cs.SearchQuery= new CB.SearchQuery();
            cs.SearchQuery.SearchOn("name", "ravi", null, "and", null, null);
            cs.Limit = 9999;
            var list = (List<CB.CloudObject>)await cs.Search();
            var table = tableNames.ToArray();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].TableName != null)
                {
                    var name = (string)list[i].TableName;
                    table = table.Where(val => val.ToString() != (string)name).ToArray();
                }
            }

            if (table.Length == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Search on both tables with OR failed.");
            }
        }

        [Test]
        public async Task minimumPercentQueries()
        {
            var obj = new CB.CloudObject("Student");
            obj.Set("name", "RAVI");
            await obj.SaveAsync();

            ArrayList tableNames = new ArrayList();
            tableNames.Add("Student");
            var cs = new CB.CloudSearch(tableNames);
            cs.SearchQuery= new CB.SearchQuery();
            cs.SearchQuery.SearchOn("name", "ravi", null, null, "75%", null);
            cs.Limit = 9999;
            var list = (List<CB.CloudObject>)await cs.Search();
            var table = tableNames.ToArray();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].TableName != null)
                {
                    var name = (string)list[i].TableName;
                    table = table.Where(val => val.ToString() != (string)name).ToArray();
                }
            }

            if (table.Length == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Search on both tables with OR failed.");
            }
        }

        [Test]
        public async Task multiTableSearch()
        {
            var obj = new CB.CloudObject("Student");
            obj.Set("name", "RAVI");
            var obj1 = new CB.CloudObject("hostel");
            obj1.Set("name", "ravi");
            await obj.SaveAsync();
            await obj1.SaveAsync();
            ArrayList tableNames = new ArrayList();
            tableNames.Add("Student");
            tableNames.Add("hostel"); 
            var cs = new CB.CloudSearch(tableNames);
            cs.SearchQuery= new CB.SearchQuery();
            cs.SearchQuery.SearchOn("name", "ravi", null, null, null, null);
            cs.Limit = 9999;
            var list = (List<CB.CloudObject>)await cs.Search();
            var table = tableNames.ToArray();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].TableName != null)
                {
                    var name = (string)list[i].TableName;
                    table = table.Where(val => val.ToString() != (string)name).ToArray();
                }
            }

            if (table.Length == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Search on both tables with OR failed.");
            }
        }

        [Test]
        public async Task shouldSaveLatitudeAndLongitude()
        {
            var obj = new CB.CloudObject("Custom5");
            var loc = new CB.CloudGeoPoint(17.7,80.0);
            obj.Set("location", loc);
            await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [Test]
        public async Task shouldIncludeRelationOnSearch()
        {
            var obj = new CB.CloudObject("Custom5");
            var loc = new CB.CloudGeoPoint(18.19, 79.3);
            loc.Latitude = 78;
            loc.Longitude = 17;
            obj.Set("location", loc);
            await obj.SaveAsync();
        }
    }
}
