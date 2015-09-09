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
        protected Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
        public CloudObject(string tableName)
        {
            dictionary.Add("_tableName", tableName);
            dictionary.Add("_type", "custom");
            dictionary.Add("_id", null);
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

            var result = await Util.CloudRequest.POST("/save", postData);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudObject> DeleteAsync()
        {

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var result = await Util.CloudRequest.POST("/delete", postData);

            this.dictionary = (Dictionary<string, Object>)result;

            return this;
        }

        public async Task<CloudObject> FetchAsync()
        {

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);
            var result = await Util.CloudRequest.POST("/" + dictionary["_tableName"]+" /get/" + dictionary["_id"], postData);
            this.dictionary = (Dictionary<string, Object>)result;
            return this;
        }
    }
}
