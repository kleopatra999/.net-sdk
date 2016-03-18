using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB
{
    public class CloudQuery
    {
        Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
        public CloudQuery(string tableName)
        { //constructor for the class CloudQueryfd
            dictionary["tableName"] = tableName;
            dictionary["query"] = new Dictionary<string, Object>();
            dictionary["$include"] = new ArrayList();
            dictionary["select"] = new Dictionary<string, Object>();
            dictionary["sort"] = new Dictionary<string, Object>();
            dictionary["skip"] = 0;
            dictionary["limit"] = 20; //limit to 20 documents by default.
        }
        // Logical operations
        public static CloudQuery Or(CloudQuery query1, CloudQuery query2)
        {
            if (query1.dictionary["tableName"].ToString()!= query2.dictionary["tableName"].ToString())
            {
                throw new Exception.CloudBoostException("Tablename of two query objects are not the same.");
            }

            var query = new CloudQuery((string)query1.dictionary["tableName"]);
            ArrayList list = new ArrayList();
            list.Add(query1);
            list.Add(query2);
            ((Dictionary<string, Object>)(query.dictionary["query"]))["$or"] = list;

            return query;
        }


        public CloudQuery EqualTo(string columnName, Object data)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }
            ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = data;

            return this;
        }

        public CloudQuery Include(string columnName)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }
            ((ArrayList)((Dictionary<string, Object>)(this.dictionary["query"]))["$include"]).Add(columnName);

            return this;
        }

        public CloudQuery NotEqualTo(string columnName, Object data)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }

            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null || ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName].GetType() == typeof(Dictionary<string, Object>))
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
            }

            ((Dictionary<string, Object>)(((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]))["$ne"] = data;

            return this;
        }

        public CloudQuery GreaterThan(string columnName, Object data)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }

            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null || ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName].GetType() == typeof(Dictionary<string, Object>))
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
            }

            ((Dictionary<string, Object>)(((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]))["$gt"] = data;

            return this;
        }
        public CloudQuery GreaterThanEqualTo(string columnName, Object data)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }

            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null || ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName].GetType() == typeof(Dictionary<string, Object>))
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
            }

            ((Dictionary<string, Object>)(((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]))["$gte"] = data;

            return this;
        }
        public CloudQuery LessThan(string columnName, Object data)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }

            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null || ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName].GetType() == typeof(Dictionary<string, Object>))
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
            }

            ((Dictionary<string, Object>)(this.dictionary[columnName]))["$lt"] = data;

            return this;
        }
        public CloudQuery LessThanEqualTo(string columnName, Object data)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }

            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null || ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName].GetType() == typeof(Dictionary<string, Object>))
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
            }

            ((Dictionary<string, Object>)(((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]))["$lte"] = data;

            return this;
        }


        //Sorting
        public CloudQuery OrderByAsc(string columnName)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }
           ((Dictionary<string, Object>)(this.dictionary["sort"]))[columnName] = 1;

            return this;
        }
        public CloudQuery OrderByDesc(string columnName)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }
           ((Dictionary<string, Object>)(this.dictionary["sort"]))[columnName] = -1;

            return this;
        }

        public int Limit
        {
            get
            {
                return (int)dictionary["limit"];
            }
            set
            {
                dictionary["limit"] = value;
            }
        }

        public int Skip
        {
            get
            {
                return (int)dictionary["skip"];
            }
            set
            {
                dictionary["skip"] = value;
            }
        }
        
        //select/deselect columns to show
        public CloudQuery SelectColumn(string columnName)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }

            ((Dictionary<string, Object>)(this.dictionary["select"]))[columnName] = 1;

            return this;
        }

        public CloudQuery SelectColumn(string[] columnNames)
        {
            for (int i = 0; i < columnNames.Length; i++)
            {
                if (columnNames[i] == "ID")
                {
                    columnNames[i] = "_id";
                }

                ((Dictionary<string, Object>)(this.dictionary["select"]))[columnNames[i]] = 1;
            }
            

            return this;
        }

        public CloudQuery DoNotSelectColumn(string columnName)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }

            ((Dictionary<string, Object>)(this.dictionary["select"]))[columnName] = 0;

            return this;
        }


        public CloudQuery DoNotSelectColumn(string[] columnNames)
        {
            for (int i = 0; i < columnNames.Length; i++)
            {
                if (columnNames[i] == "ID")
                {
                    columnNames[i] = "_id";
                }

                ((Dictionary<string, Object>)(this.dictionary["select"]))[columnNames[i]] = 0;
            }

            return this;
        }

        public CloudQuery ContainedIn(string columnName, object data)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }

            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null || ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName].GetType() == typeof(Dictionary<string, Object>))
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
            }

            if(((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$in"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$in"] = new ArrayList();
            }

            ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$in"]).Add(data);

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]) != null && ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$nin"]) != null && ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$nin"]).GetType() == typeof(ArrayList))
            {
                if (((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$nin"]).IndexOf(data) > -1)
                {
                    //remove from not contained in list
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$nin"]).Remove(data);
                }
            }

            return this;
        }

        public CloudQuery ContainedIn(string columnName, string[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                ContainedIn(columnName, data[i]);
            }

            return this;
        }

        public CloudQuery NotContainedIn(string columnName, string data)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }

            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null || ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName].GetType() == typeof(Dictionary<string, Object>))
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$nin"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$nin"] = new ArrayList();
            }

            ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$nin"]).Add(data);

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]) != null && ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$in"]) != null && ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$in"]).GetType() == typeof(ArrayList))
            {
                if (((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$in"]).IndexOf(data) > -1)
                {
                    //remove from not contained in list
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName])["$in"]).Remove(data);
                }
            }

            return this;
        }

        public CloudQuery NotContainedIn(string columnName, string[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                NotContainedIn(columnName, data[i]);
            }

            return this;
        }


        public CloudQuery Exists(string columnName)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }

            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null || ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName].GetType() == typeof(Dictionary<string, Object>))
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
            }

           ((Dictionary<string, Object>)(((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]))["$exists"] = true;

            return this;
        }

        public CloudQuery DoesNotExist(string columnName)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }

            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null || ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName].GetType() == typeof(Dictionary<string, Object>))
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
            }

           ((Dictionary<string, Object>)(((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]))["$exists"] = false;

            return this;
        }

        public CloudQuery ContainsAll(string columnName, Object[] values)
        {
            if (columnName == "ID")
            {
                columnName = "_id";
            }

            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null || ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName].GetType() == typeof(Dictionary<string, Object>))
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
            }

           (((Dictionary<string, Object>)((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]))["$all"] = values;

            return this;
        }

        public CloudQuery StartsWith(string columnName, string value)
        {
            var regex = '^' + value;

            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null || ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName].GetType() == typeof(Dictionary<string, Object>))
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
            }

            if (((Dictionary<string, Object>)(this.dictionary["query"])) != null)
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))["$regex"] = regex;
                ((Dictionary<string, Object>)(this.dictionary["query"]))["$options"] = "im";
            }

            return this;
        }

        //GeoPoint near query
        public CloudQuery near(string columnName, CB.CloudGeoPoint geoPoint, double maxDistance, double minDistance)
        {
            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null)
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
                Dictionary<string, object> near = new Dictionary<string,object>();
                Dictionary<string, object> geometry = new Dictionary<string,object>();
                double[] coordinates = (double[])geoPoint.dictionary["coordinates"];
                geometry.Add("coordinates", coordinates);
                geometry.Add("type", "Point");
                near.Add("$geometry", geometry);
                near.Add("$maxDistance", maxDistance);
                near.Add("$minDistance", minDistance);
                ((Dictionary<string, Object>)(((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]))["$near"] = near;
            }
            return this;
        }

        public CloudQuery Near(string columnName, CB.CloudGeoPoint geoPoint, double maxDistance)
        {
            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null)
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
                Dictionary<string, object> near = new Dictionary<string,object>();
                Dictionary<string, object> geometry = new Dictionary<string,object>();
                double[] coordinates = (double[])geoPoint.dictionary["coordinates"];
                geometry.Add("coordinates", coordinates);
                geometry.Add("type", "Point");
                near.Add("$geometry", geometry);
                near.Add("$maxDistance", maxDistance);
                near.Add("$minDistance", null);
                ((Dictionary<string, Object>)(((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]))["$near"] = near;
            }
            return this;
        }

        //GeoPoint geoWithin query
        public CloudQuery GeoWithin(string columnName, CloudGeoPoint[] geoPoint)
        {
            double[][] coordinates = {};
            for(int i=0; i<geoPoint.Length; i++)
            {
                coordinates[i] = (double[])geoPoint[i].dictionary["coordinates"];
            }
            string type = "Polygon";
            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null)
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
                Dictionary<string, object> geoWithin = new Dictionary<string,object>();
                Dictionary<string, object> geometry = new Dictionary<string,object>();
                geometry.Add("coordinates", coordinates);
                geometry.Add("type", type);
                geoWithin.Add("$geometry", geometry);
                ((Dictionary<string, Object>)(((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]))["$geoWithin"] = geoWithin;
            }

            return this;
        }

        public CloudQuery GeoWithin(string columnName, CloudGeoPoint geoPoint, double radius)
        {
            double[] coordinates = (double[])geoPoint.dictionary["coordinates"];
            
            if (((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] == null)
            {
                ((Dictionary<string, Object>)(this.dictionary["query"]))[columnName] = new Dictionary<string, Object>();
                Dictionary<string, object> geoWithin = new Dictionary<string,object>();
                Dictionary<string, object> centerSphere = new Dictionary<string,object>();
                centerSphere.Add("coordinates", coordinates);
                centerSphere.Add("radius", radius/3963.2);
                geoWithin.Add("$geometry", centerSphere);
                ((Dictionary<string, Object>)(((Dictionary<string, Object>)(this.dictionary["query"]))[columnName]))["$geoWithin"] = geoWithin;
            }

            return this;
        }

        public async Task<int> CountAsync()
        {
            var postData = new Dictionary<string, Object>();
            postData["query"] = this.dictionary["query"];
            postData["limit"] = this.dictionary["limit"];
            postData["skip"] = this.dictionary["skip"];
            var result = await Util.CloudRequest.SendObject(Util.CloudRequest.Method.POST, CloudApp.ApiUrl + "/" + this.dictionary["tableName"] + "/count", this.dictionary, false);
            return (int)result;
        }

        public async Task<List<CB.CloudObject>> DistinctAsync(string key)
        {
            var postData = new Dictionary<string, Object>();
            postData["onKey"] = key;
            postData["query"] = this.dictionary["query"];
            postData["select"] = this.dictionary["select"];
            postData["sort"] = this.dictionary["sort"];
            postData["limit"] = this.dictionary["limit"];
            postData["skip"] = this.dictionary["skip"];
            var result = await Util.CloudRequest.SendArray(Util.CloudRequest.Method.POST, "/" + this.dictionary["tableName"] + "/distinct", postData, false);
            List<CloudObject> list = CB.PrivateMethods.ToCloudObjectList(result);
            return list;
        }

        public async Task<List<CB.CloudObject>> FindAsync()
        {
            var postData = new Dictionary<string, Object>();
            postData["query"] = this.dictionary["query"];
            postData["select"] = this.dictionary["select"];
            postData["sort"] = this.dictionary["sort"];
            postData["limit"] = this.dictionary["limit"];
            postData["skip"] = this.dictionary["skip"];

            var result = await Util.CloudRequest.SendArray(Util.CloudRequest.Method.POST, "/ " + this.dictionary["tableName"] + "/find", postData, false);
            List<CloudObject> list = CB.PrivateMethods.ToCloudObjectList(result);
            return list;
        }

        public async Task<Dictionary<string, Object>> PaginateAsync(int pageNo, int totalItemsInPage)
        {
            if (pageNo > 0)
            {
                if (totalItemsInPage > 0)
                {
                    int skip = (pageNo * totalItemsInPage) - totalItemsInPage;
                    this.Skip = skip;
                    this.Limit = totalItemsInPage;
                }


            }

            if (totalItemsInPage > 0)
            {
                this.Limit = totalItemsInPage;
            }

            var findTask = Util.CloudRequest.SendArray(Util.CloudRequest.Method.POST, "/ " + this.dictionary["tableName"] + "/find", this.dictionary, false);
            var countObj = new CB.CloudQuery(this.dictionary["tableName"].ToString());
            countObj.dictionary = this.dictionary;
            var countTask = Util.CloudRequest.SendObject(Util.CloudRequest.Method.POST, CloudApp.ApiUrl + "/" + this.dictionary["tableName"] + "/count", countObj.dictionary, false);
            await Task.WhenAll(findTask, countTask);
            var findResult = await findTask;
            List<CloudObject> list = CB.PrivateMethods.ToCloudObjectList(findResult);
            var countResult = await countTask;
            var count = (int)countResult;
            int totalPages = 0;
            if (count != null)
            {
                count = 0;
                totalPages = 0;
            }
            else
            {
                totalPages = (int)(count / this.Limit);
            }
            if (totalPages < 0)
            {
                totalPages = 0;
            }

            var resultObject = new Dictionary<string, Object>();
            resultObject.Add("objectsList", findResult);
            resultObject.Add("count", count);
            resultObject.Add("totalPages", totalPages);
            return resultObject;
        }

        public async Task<CB.CloudObject> GetAsync(string objectId)
        {
            var postData = new Dictionary<string, Object>();
            postData["query"] = this.dictionary["query"];
            postData["select"] = this.dictionary["select"];
            postData["limit"] = 1;
            postData["skip"] = 0;
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, "/ " + this.dictionary["tableName"] + "/get/"+ objectId, postData, false);
            var obj = new CloudObject(result["name"].ToString());
            obj.dictionary = result;
            return obj;
        }

        public async Task<CloudObject> FindOneAsync()
        {
            var postData = new Dictionary<string, Object>();
            postData["query"] = this.dictionary["query"];
            postData["select"] = this.dictionary["select"];
            postData["sort"] = this.dictionary["sort"];
            postData["skip"] = this.dictionary["skip"];
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, "/ " + this.dictionary["tableName"] + "/findOne" , postData, false);
            var obj = new CloudObject(result["name"].ToString());
            obj.dictionary = result;
            return obj;
        }
    }
}
