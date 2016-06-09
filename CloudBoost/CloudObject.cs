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
        internal Dictionary<string, Object> dictionary { set; get; }

        public CloudObject(string tableName)
        {
            this.dictionary = new Dictionary<string, Object>();
            dictionary.Add("_tableName", tableName);
            dictionary.Add("_type", "custom");
            dictionary.Add("ACL", new CB.ACL());
            dictionary.Add("expires", null);
            dictionary["_modifiedColumns"] = new ArrayList();
            ((ArrayList)dictionary["_modifiedColumns"]).Add("createdAt");
            ((ArrayList)dictionary["_modifiedColumns"]).Add("updatedAt");
            ((ArrayList)dictionary["_modifiedColumns"]).Add("ACL");
            ((ArrayList)dictionary["_modifiedColumns"]).Add("expires");
            dictionary.Add("_isModified", true);

        }

        public CB.ACL ACL
        {
            get
            {
                if (dictionary.ContainsKey("ACL") == true && dictionary["ACL"] != null)
                {
                    return (CB.ACL)dictionary["ACL"];
                }

                return null;
            }
            set
            {
                if (value.GetType() == typeof(CB.ACL))
                {
                    dictionary["ACL"] = value;
                    _IsModified(this, "ACL");
                }    
                else
                    throw new Exception.CloudBoostException("Value is not of type ACL");
            }
        }

        public string ID
        {
            get
            {
                if (dictionary.ContainsKey("_id") == true && dictionary["_id"]!=null)
                {
                    return (string)dictionary["_id"];
                }

                return null;
                
            }
            set
            {
                if (value.GetType() == typeof(string))
                {
                    dictionary["_id"] = value;
                    _IsModified(this, "_id");
                }
                else
                    throw new Exception.CloudBoostException("Value is not of type string");
            }

        }

        public DateTime? CreatedAt
        {
            get
            {
                if (dictionary.ContainsKey("createdAt") && dictionary["createdAt"] != null)
                    return (DateTime)dictionary["createdAt"];
                else
                    return null;
            }
            set
            {
                if (value.GetType() == typeof(DateTime))
                {
                    dictionary["createdAt"] = value;
                    _IsModified(this, "createdAt");
                }
                else
                    throw new Exception.CloudBoostException("Value is not of type DateTime");
            }

        }


        public string TableName
        {
            get
            {
                if (dictionary.ContainsKey("_tableName") && dictionary["_tableName"] != null)
                    return (string)dictionary["_tableName"];
                else
                    return null;
            }
            set
            {
                if (value.GetType() == typeof(string))
                {
                    dictionary["_tableName"] = value;
                    _IsModified(this, "_tableName");
                }
                else
                    throw new Exception.CloudBoostException("Value is not of type string");
            }

        }

        public DateTime? UpdatedAt
        {
            get
            {
                if (dictionary.ContainsKey("updatedAt") && dictionary["updatedAt"] != null)
                    return (DateTime)dictionary["updatedAt"];
                else
                    return null;
            }
            set
            {
                if (value.GetType() == typeof(DateTime))
                {
                    dictionary["updatedAt"] = value;
                    _IsModified(this, "updatedAt");
                }
                else
                    throw new Exception.CloudBoostException("Value is not of type DateTime");
            }

        }

        public DateTime? Expires
        {
            get
            {
                if (dictionary.ContainsKey("expires")==true && dictionary["expires"]!=null)
                    return (DateTime)dictionary["expires"];
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    dictionary["expires"] = value;
                    _IsModified(this, "expires");
                }                
            }

        }

        public void Set(string columnName, Object value)
        {

            ArrayList keywords = new ArrayList();
            keywords.Add("_tableName");
            keywords.Add("_type");
            keywords.Add("operator");
            keywords.Add("_id");

            if (columnName.ToUpper() == "ID" || columnName == "IsSearchable")
                columnName = "_" + ((char)columnName.ToCharArray()[0]).ToString().ToLower() + columnName.Substring(1);

            if (keywords.IndexOf(columnName) > -1)
            {
                throw new Exception.CloudBoostException(columnName + " is a keyword. Please choose a different column name.");
            }

            dictionary[columnName] = value;
            _IsModified(this, columnName);
        }

        public Object Get(string columnName)
        {
            if (dictionary.ContainsKey(columnName))
                return dictionary[columnName];
            else
                return null;
        }

        public T Get<T>(string columnName)
        {
            if (dictionary.ContainsKey(columnName))
                return (T)Convert.ChangeType(dictionary[columnName], typeof(T));
            else
                return default(T);
        }

        public void Unset(string columnName)
        { 
            dictionary[columnName] = null;
            _IsModified(this, columnName);
        }

        public static void On(string tableName, string eventType, Callback callback)
        {
            tableName = tableName.ToLower();
            eventType = eventType.ToUpper();
            if (eventType == "created" || eventType == "updated" || eventType == "deleted")
            {
                string str = (CloudApp.AppID + "table" + tableName + eventType).ToLower();
                Dictionary<string, object> payload = new Dictionary<string, object>();
                payload.Add("room", str);
                payload.Add("sessionId", CB.PrivateMethods._getSessionId());
                CloudApp._socket.Emit("join-object-channel", payload);
                CloudApp._socket.On(str, (data) =>
                {
                    var result = (Dictionary<string, Object>)data;
                    var cbObj = new CB.CloudObject(result["_tableName"].ToString());
                    cbObj.dictionary = result;
                    callback(result);
                });
            }
        }

        public static void On(string tableName, string[] eventType, Callback callback)
        {
            for (int i = 0; i < eventType.Length; i++)
            {
                CloudObject.On(tableName, eventType[i], callback);
            }
        }

        public static void Off(string tableName, string eventType)
        {
            tableName = tableName.ToLower();
            eventType = eventType.ToUpper();
            if (eventType == "created" || eventType == "updated" || eventType == "deleted")
            {
                string str = (CloudApp.AppID + "table" + tableName + eventType).ToLower();
                Dictionary<string, object> payload = new Dictionary<string, object>();
                payload.Add("room", str);
                payload.Add("sessionId", CB.PrivateMethods._getSessionId());
                CloudApp._socket.Emit("leave-object-channel", payload);
                CloudApp._socket.Off(str);
            }
        }

        public static void Off(string tableName, string[] eventType)
        {
            for (int i = 0; i < eventType.Length; i++)
            {
                CloudObject.Off(tableName, eventType[i]);
            }
        }

        public void Relate(string columnName, string objectTableName, string objectId)
        {
            ArrayList keywords = new ArrayList();
            keywords.Add("_tableName");
            keywords.Add("_type");
            keywords.Add("operator");
            if (columnName == "id" || columnName == "_id")
            {
                throw new CB.Exception.CloudBoostException("You cannot link an object to this column");
            }

            if (keywords.Contains(columnName))
            {
                throw new CB.Exception.CloudBoostException(columnName + " is a keyword. Please choose a different column name.");
            }

            this.dictionary[columnName] = new CB.CloudObject(objectTableName);
            _IsModified(this, columnName);
        }
        public async Task<CloudObject> SaveAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData["document"] = dictionary;
            string url = CloudApp.ApiUrl + "/data/" + CloudApp.AppID + "/" + dictionary["_tableName"];

            Dictionary<string, Object> result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.PUT, url, postData);

            this.dictionary = result;

            return this;
        }

        public async Task<CloudObject> DeleteAsync()
        {

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this.dictionary);
            postData.Add("method", "DELETE");
       

            var url = CloudApp.ApiUrl + "/data/" + CloudApp.AppID + "/" + dictionary["_tableName"];

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.PUT, url, postData);

            dictionary = result;

            return this;
        }

        public async Task<CloudObject> FetchAsync()
        {
            if (dictionary["_id"] == null)
            {
                throw new Exception.CloudBoostException("Can't fetch an object which is not saved");
            }

            var query = new CloudQuery(dictionary["_tableName"].ToString());

            if (dictionary["_type"].ToString() == "file")
            {
                query = new CloudQuery("_File");
            }

            CB.CloudObject obj = await query.GetAsync(dictionary["_id"].ToString());

            return obj;
        }

        public static async Task<List<CloudObject>> SaveAllAsync(ArrayList objectArray)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            var array = new ArrayList();
            for (int i = 0; i < objectArray.Count; i++)
            {
                array.Add(((CloudObject)objectArray[i]).dictionary);
            }
            postData.Add("document", array);

            var url = CloudApp.ApiUrl + "/data/" + CloudApp.AppID + "/" + ((CB.CloudObject)array[0]).TableName;

            var result = await Util.CloudRequest.Send<List<Dictionary<string, Object>>>(Util.CloudRequest.Method.PUT, url, postData);

            List<CloudObject> objects = new List<CloudObject>();

            var objectList = (List<Dictionary<string, Object>>)result;

            for (int i = 0; i < objectList.Count; i++)
            {
                var obj = new CloudObject(objectList[i]["_tableName"].ToString());
                obj.dictionary = objectList[i];
                objects.Add(obj);
            }

            return objects;
        }

        public static async Task<List<CloudObject>> DeleteAllAsync(ArrayList objectArray)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            var array = new ArrayList();
            for (int i = 0; i < objectArray.Count; i++)
            {
                array.Add(((CloudObject)objectArray[i]).dictionary);
            }
            postData.Add("document", array);

            var url = CloudApp.ApiUrl + "/data/" + CloudApp.AppID + "/" + ((CB.CloudObject)array[0]).TableName;

            var result = await Util.CloudRequest.Send<List<Dictionary<string, Object>>>(Util.CloudRequest.Method.DELETE, url, postData);

            List<CloudObject> objects = new List<CloudObject>();

            var objectList = (List<Dictionary<string, Object>>)result;

            for (int i = 0; i < objectList.Count; i++)
            {
                var obj = new CloudObject(objectList[i]["_tableName"].ToString());
                obj.dictionary = objectList[i];
                objects.Add(obj);
            }

            return objects;
        }

        protected static void _IsModified(CB.CloudObject cbObj, string columnName)
        {
            cbObj.dictionary["_isModified"] = true;

            if (cbObj.dictionary["_modifiedColumns"] == null)
            {
                cbObj.dictionary["_modifiedColumns"] = new ArrayList();
                ((ArrayList)cbObj.dictionary["_modifiedColumns"]).Add(columnName);
            }
            else if (((ArrayList)cbObj.dictionary["_modifiedColumns"]).Contains(columnName) == false)
            {
                ((ArrayList)cbObj.dictionary["_modifiedColumns"]).Add(columnName);
            }

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
