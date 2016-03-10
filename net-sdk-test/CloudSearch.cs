using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CB.Test
{
    [TestClass]
    public class CloudSearch
    {
        [TestMethod]
        public void x001_InitAppWithMasterKey()
        {
            var tableName = CB.Test.Util.Methods._makeString();

            CB.Test.Util.Keys.InitWithMasterKey();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task getDataFromServerNearFunction()
        {
            var custom = new CB.CloudTable("CustomGeoPoint");
            var newColumn7 = new CB.Column("location");
            newColumn7.DataType= CB.DataType.GeoPoint;
            custom.AddColumn(newColumn7);
            var response = await custom.SaveAsync();
            CB.CloudApp.AppKey = CB.CloudApp.JsKey;
            var loc = new CB.CloudGeoPoint(17.7,80.0);
            var obj = new CB.CloudObject("CustomGeoPoint");
            obj.Set("location", loc);
            await obj.SaveAsync();
            var search = new CB.CloudSearch("CustomGeoPoint");
            
            search.SearchFilter = new CB.SearchFilter();
            search.SearchFilter.Near("location", loc, 1);
            var list = (List<CloudObject>)await search.Search();
            if(list.Count > 0)
            {
                Assert.IsTrue(true);
            }else
            {
                throw new CB.Exception.CloudBoostException("should have retrieved data");
            }
        }

        [TestMethod]
        public async Task EqualToWithCloudSearchOverCloudObject()
        {
            var custom = new CB.CloudTable("CustomRelation");
            var newColumn1 = new CB.Column("newColumn7");
            newColumn1.DataType = CB.DataType.Relation;
            custom.AddColumn(newColumn1);
            await custom.SaveAsync();
            CB.CloudApp.AppKey = CB.CloudApp.JsKey;
            var loc = new CB.CloudGeoPoint(17.7, 80.0);
            var obj = new CB.CloudObject("CustomRelation");
            var obj1 = new CB.CloudObject("student1");
            obj1.Set("name", "Ranjeet");
            obj.Set("newColumn7", obj1);
            await obj.SaveAsync();
            var search = new CB.CloudSearch("CustomRelation");
            search.SearchFilter = new CB.SearchFilter();
            search.SearchFilter.EqualTo("newColumn7", obj.Get("newColumn7"));
            var list = (List<CloudObject>)await search.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("should have retrieved data");
            }
        }


        [TestMethod]
        public async Task indexObject()
        {
            var obj = new CB.CloudObject("Custom1");
            obj.set("description", "wi-fi");
            await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task searchIndexedObject()
        {
            var cs = new CB.CloudSearch("Custom1");
            cs.SearchQuery = new CB.SearchQuery();
            cs.SearchQuery.SearchOn("description", "wi-fi", null, null, null, null);
            var list = (List<CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
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

        [TestMethod]
        public async Task searchObjectForGivenValue()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();

            cs.SearchFilter.EqualTo("age", 19);
            var list = (List<CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task searchObjectWithPhrase()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchQuery = new CB.SearchQuery();

            cs.SearchQuery.Phrase("name", "Gautam Singh", null, null);
            var list = (List<CloudObject>)await cs.Search();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task searchObjectWithWhileCard()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchQuery = new CB.SearchQuery();

            cs.SearchQuery.WildCard("name", "G*", null);
            var list = (List<CloudObject>)await cs.Search();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task searchForObjectWithStartWith()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchQuery = new CB.SearchQuery();

            cs.SearchQuery.StartsWith("name", "G", null);
            var list = (List<CloudObject>)await cs.Search();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task searchObjectWithMostColumns()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchQuery = new CB.SearchQuery();
            string[] column = { "name", "description" };
            cs.SearchQuery.MostColumns(column, "G", null, null, null, null);
            var list = (List<CloudObject>)await cs.Search();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task searchForObjectWithBestColumns()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchQuery= new CB.SearchQuery();
            string[] column = { "name", "description" };
            cs.SearchQuery.BestColumns(column, "G", null, null, null, null);
            var list = (List<CloudObject>)await cs.Search();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task shouldSearchWithNotEqualToValue()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();
            cs.SearchFilter.NotEqualTo("age", 19);
            var list = (List<CloudObject>)await cs.Search();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task setLimitTest()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();
            cs.SearchFilter.NotEqualTo("age", 19);
            cs.Limit(0);
            var list = (List<CloudObject>)await cs.Search();
            if (list.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("should limit the number of results");
            }

        }

        [TestMethod]
        public async Task skipTest()
        {
            var obj = new CB.CloudObject("Student");
            obj.Set("age",21);
            await obj.SaveAsync();
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();
            cs.SearchFilter.NotEqualTo("age", 19);
            cs.Skip(999999);
            var list = (List<CloudObject>)await cs.Search();
            if (list.Count == 0)
            {
                var cs = new CB.CloudSearch("Student");
                cs.SearchFilter = new CB.SearchFilter();
                cs.SearchFilter.NotEqualTo("age", 19);
                cs.Skip(1);
                var list = (List<CloudObject>)await cs.Search();
                if (list.Count > 0)
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    throw new CB.Exception.CloudBoostException("");
                }
            }
            else
            {
                throw new CB.Exception.CloudBoostException("");
            }
        }

        [TestMethod]
        public async Task sortAscendingOrder()
        {
            var cs = new CB.CloudSearch("Student");
            cs.OrderByAsc("age");
            var list = (List<CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("");
            }
        }

        [TestMethod]
        public async Task sortInDescendingOrder()
        {
            var cs = new CB.CloudSearch("Student");
            cs.OrderByDesc("age");
            var list = (List<CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("");
            }
        }

        [TestMethod]
        public async Task columnExistsTest()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();
            cs.SearchFilter.Exists("name");
            var list = (List<CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("");
            }
        }

        [TestMethod]
        public async Task searchRecordsWhichDoNotHaveCertainColumnTest()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();
            cs.SearchFilter.doesNotExist("expire");
            var list = (List<CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("");
            }
        }

        [TestMethod]
        public async Task recordsWithinCertainRange()
        {
            var cs = new CB.CloudSearch("Student");
            cs.SearchFilter = new CB.SearchFilter();
           
            cs.SearchFilter.GreaterThan("age",19);
            cs.SearchFilter.LessThan("age",50);
            var list = (List<CloudObject>)await cs.Search();
            if (list.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("");
            }
        }

        [TestMethod]
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

            var list = (List<CloudObject>)await cs.Search();
            var table = tableNames.ToArray();
            for (int i = 0; i < list.Count; i++)
            {

                if (list[i].dictionary["_tableName"] != null)
                {
                    var name = (string)list[i].dictionary["_tableName"];
                     
                    table = table.Where(val => val != name).ToArray();
                }
            }

            if (table.Length == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("Search on both tables with OR failed.");
            }
        }

        [TestMethod]
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
            var list = (List<CloudObject>)await cs.Search();
            var table = tableNames.ToArray();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].dictionary["_tableName"] != null)
                {
                    var name = (string)list[i].dictionary["_tableName"];
                    table = table.Where(val => val != name).ToArray();
                }
            }

            if (table.Length == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("Search on both tables with OR failed.");
            }
        }

        [TestMethod]
        public async Task minimumPercentQueries()
        {
            var obj = new CB.CloudObject("Student");
            obj.Set("name", "RAVI");
            await obj.SaveAsync();

            ArrayList tableNames = new ArrayList();
            tableNames.Add("Student");
            var cs = new CB.CloudSearch(tableNames);
            cs.SearchQuery= new CB.SearchQuery();
            cs.SearchQuery.SearchOn("name", "ravi", null, null, "75%");
            cs.Limit = 9999;
            var list = (List<CloudObject>)await cs.Search();
            var table = tableNames.ToArray();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].dictionary["_tableName"] != null)
                {
                    var name = (string)list[i].dictionary["_tableName"];
                    table = table.Where(val => val != name).ToArray();
                }
            }

            if (table.Length == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("Search on both tables with OR failed.");
            }
        }

        [TestMethod]
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
            var list = (List<CloudObject>)await cs.Search();
            var table = tableNames.ToArray();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].dictionary["_tableName"] != null)
                {
                    var name = (string)list[i].dictionary["_tableName"];
                    table = table.Where(val => val != name).ToArray();
                }
            }

            if (table.Length == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("Search on both tables with OR failed.");
            }
        }

        [TestMethod]
        public async Task shouldSaveLatitudeAndLongitude()
        {
            var obj = new CB.CloudObject("Custom5");
            var loc = new CB.CloudGeoPoint(17.7,80.0);
            obj.Set("location", loc);
            await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task shouldIncludeRelationOnSearch()
        {
            var obj = new CB.CloudObject("Custom5");
            var loc = new CB.CloudGeoPoint(18.19, 79.3);
            loc.SetLatitude(78);
            loc.SetLongitude(17);
            obj.Set("location", loc);
            await obj.SaveAsync();
        }
    }
}
