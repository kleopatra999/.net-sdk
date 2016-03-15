using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CB.Test
{
    [TestClass]
    public class CloudGeoPoint
    {
        [TestMethod]
        public async Task saveGeoPoint()
        {
            var obj = new CB.CloudObject("Custom5");
            var loc = new CB.CloudGeoPoint(17.7, 78.9);
            obj.Set("location", loc);
            await obj.SaveAsync();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void createGeoPointWithZero()
        {
            var loc = new CB.CloudGeoPoint(0, 0);
        }

        [TestMethod]
        public async Task nearTest()
        {
            var query = new CB.CloudQuery("Custom5");
            var loc = new CB.CloudGeoPoint(17.7, 78.9);
            query.Near("location", loc, 400000);
            var response = (List<CB.CloudObject>) await query.Find();
            if (response.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("should retrieve saved data with particular value");
            }
        }

        [TestMethod]
        public async Task geoWithinTest()
        {
            var query = new CB.CloudQuery("Custom5");
            var loc1 = new CB.CloudGeoPoint(18.4, 78.9);
            var loc2 = new CB.CloudGeoPoint(17.4, 78.4);
            var loc3 = new CB.CloudGeoPoint(17.7, 80.4);
            CB.CloudGeoPoint[] loc = { loc1 ,loc2, loc3};

            query.GeoWithin("location", loc);
            var response = (List<CB.CloudObject>)await query.Find();
            if (response.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("should retrieve saved data with particular value");
            }
        }
        public async Task geoWithinTestLimit()
        {
            var query = new CB.CloudQuery("Custom5");
            var loc1 = new CB.CloudGeoPoint(18.4, 78.9);
            var loc2 = new CB.CloudGeoPoint(17.4, 78.4);
            var loc3 = new CB.CloudGeoPoint(17.7, 80.4);
            CB.CloudGeoPoint[] loc = { loc1, loc2, loc3 };
            query.Limit = 4;
            query.GeoWithin("location", loc);
            var response = (List<CB.CloudObject>)await query.Find();
            if (response.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("should retrieve saved data with particular value");
            }
        }

        public async Task geoWithinTestCircle()
        {
            var query = new CB.CloudQuery("Custom5");
            var loc = new CB.CloudGeoPoint(17.3, 78.3);
            query.Limit = 4;
            query.GeoWithin("location", loc, 1000);
            var response = (List<CB.CloudObject>)await query.Find();
            if (response.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new CB.Exception.CloudBoostException("should retrieve saved data with particular value");
            }
        }

        public async Task updateGeoPoint()
        {
            var obj = new CB.CloudObject("Custom5");
            var loc = new CB.CloudGeoPoint(17.7, 78.9);
            obj.Set("location", loc);
            await obj.SaveAsync();
            Assert.IsTrue(true);
            obj.Set("latitude", 55);
            await obj.SaveAsync();
            Assert.IsTrue(true);
        }
    }
}
