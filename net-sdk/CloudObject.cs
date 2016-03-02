using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace CB
{
    public class CloudObject
    {
        internal Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
        public CloudObject(string tableName)
        {
            dictionary.Add("_tableName", tableName);
            dictionary.Add("_type", "custom");
            dictionary.Add("_id", null);
            dictionary.Add("ACL", new CB.ACL());
        }

        public CloudObject(string tableName, string id)
        {
            dictionary.Add("_tableName", tableName);
            dictionary.Add("_type", "custom");
            dictionary.Add("_id", id);
            dictionary.Add("ACL", new CB.ACL());
        }

        public CB.ACL ACL
        {
            get
            {
                return (CB.ACL)dictionary["ACL"];
            }
            set
            {
                if (value.GetType() == typeof(CB.ACL))
                    dictionary["ACL"] = value;
                else
                    throw new Exception.CloudBoostException("Value is not of type ACL");
            }
        }

        public string ID
        {
            get
            {
                return (string)dictionary["_id"];
            }
            set
            {
                if (value.GetType() == typeof(string))
                    dictionary["_id"] = value;
                else
                    throw new Exception.CloudBoostException("Value is not of type string");
            }

        }

        public DateTime CreatedAt
        {
            get
            {
                return (DateTime)dictionary["createdAt"];
            }
            set
            {
                if (value.GetType() == typeof(DateTime))
                    dictionary["createdAt"] = value;
                else
                    throw new Exception.CloudBoostException("Value is not of type DateTime");
            }

        }

        public string TableName
        {
            get
            {
                return (string)dictionary["_tableName"];
            }
            set
            {
                if (value.GetType() == typeof(string))
                    dictionary["_tableName"] = value;
                else
                    throw new Exception.CloudBoostException("Value is not of type string");
            }

        }

        public DateTime UpdatedAt
        {
            get
            {
                return (DateTime)dictionary["updatedAt"];
            }
            set
            {
                if (value.GetType() == typeof(DateTime))
                    dictionary["updatedAt"] = value;
                else
                    throw new Exception.CloudBoostException("Value is not of type DateTime");
            }

        }

        public bool IsSearchable
        {
            get
            {
                return (bool)dictionary["_isSearchable"];
            }
            set
            {
                if (value.GetType() == typeof(bool))
                    dictionary["_isSearchable"] = value;
                else
                    throw new Exception.CloudBoostException("Value is not of type bool");
            }

        }

        public Object Get(string columnName)
        { //for getting data of a particular column

            if (columnName == "ID" || columnName == "IsSearchable")
                columnName = "_" + ((char)columnName.ToCharArray()[0]).ToString().ToLower() + columnName.Substring(1);


            return dictionary[columnName];
        }

        public void Set(string columnName, Object value)
        {

            ArrayList keywords = new ArrayList();
            keywords.Add("_tableName");
            keywords.Add("_type");
            keywords.Add("operator");

            if (columnName == "ID" || columnName == "IsSearchable")
                columnName = "_" + ((char)columnName.ToCharArray()[0]).ToString().ToLower() + columnName.Substring(1);

            if (keywords.IndexOf(columnName) > -1)
            {
                throw new Exception.CloudBoostException(columnName + " is a keyword. Please choose a different column name.");
            }

            dictionary[columnName] = value;
        }

        public void Unset(string columnName)
        { 
            dictionary[columnName] = null;
        }

        public async Task<CloudObject> SaveAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var url = CloudApp.ApiUrl + "/data/" + CloudApp.AppID + "/" + postData["_tableName"];

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.PUT, url, postData, false);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudObject> DeleteAsync()
        {

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var url = CloudApp.ApiUrl + "/data/" + CloudApp.AppID + "/" + postData["_tableName"];

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.DELETE, url, postData, false);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudObject> FetchAsync()
        {
            if (dictionary["_id"] == null)
            {
                throw new Exception.CloudBoostException("Can't fetch an object which is not saved");
             
                //return this;
            }

            var query = new CloudQuery(dictionary["_tableName"].ToString());

            if (dictionary["_type"] == "file")
            {
                query = new CloudQuery("_File");
            }

            CloudObject obj = await query.Get(dictionary["_id"].ToString());

            return obj;
        }

        public static async Task<List<CloudObject>> SaveAllAsync(CloudObject[] objectArray)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", objectArray);

            var url = CloudApp.ApiUrl + "/data/" + CloudApp.AppID + "/" + postData["_tableName"];

            var result = await Util.CloudRequest.SendArray(Util.CloudRequest.Method.PUT, url, postData, false);

            List<CloudObject> objects = new List<CloudObject>();

            var objectList = (List<Dictionary<string, Object>>)result;

            for (int i = 0; i < objectList.Count; i++)
            {
                var obj = new CloudObject(objectList[i]["_tableName"].ToString(), objectList[i]["_id"].ToString());
                obj.dictionary = objectList[i];
                objects.Add(obj);
            }

            return objects;
        }

        public static async Task<List<CloudObject>> DeleteAllAsync(CloudObject[] objectArray)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", objectArray);

            var url = CloudApp.ApiUrl + "/data/" + CloudApp.AppID + "/" + postData["_tableName"];

            var result = await Util.CloudRequest.SendArray(Util.CloudRequest.Method.DELETE, url, postData, false);

            List<CloudObject> objects = new List<CloudObject>();

            var objectList = (List<Dictionary<string, Object>>)result;

            for (int i = 0; i < objectList.Count; i++)
            {
                var obj = new CloudObject(objectList[i]["_tableName"].ToString(), objectList[i]["_id"].ToString());
                obj.dictionary = objectList[i];
                objects.Add(obj);
            }

            return objects;
        }

        internal static bool Modified(CB.CloudObject obj, string columnName)
        {

            List<Object> modifiedColumns = new List<Object>();
            List<Object> col = new List<Object>();
       
            try
            {
                col = obj.dictionary.Select(_modifiedColumns => _modifiedColumns.Value).ToList();
            }
            catch (IndexOutOfRangeException e2)
            {

                throw e2;
            }
            for (int i = 0; i < col.Count; i++)
            {
                try
                {
                    modifiedColumns.Add(col.ElementAt(i));
                }
                catch (IndexOutOfRangeException e)
                {

                    throw new IndexOutOfRangeException(e.Message);
                }
                catch (CB.Exception.CloudBoostException e)
                {

                    throw new CB.Exception.CloudBoostException(e.Message);
                }
            }
            try
            {
                obj.dictionary.Add("_isModified", true);
            }
            catch (CB.Exception.CloudBoostException e1)
            {

                throw new CB.Exception.CloudBoostException(e1.Message); ;
            }

            if (modifiedColumns.Contains(columnName))
            {
                modifiedColumns.Clear();
                modifiedColumns.Add(columnName);
            }
            else
            {
                modifiedColumns.Add(columnName);
            }
            try
            {
                obj.dictionary.Add("_modifiedColumns", modifiedColumns);
            }
            catch (IndexOutOfRangeException e)
            {

                throw new IndexOutOfRangeException(e.Message);
            }
            return true;
        }
    }
}
