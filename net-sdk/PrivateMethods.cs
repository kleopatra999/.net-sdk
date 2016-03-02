using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CB
{
    class PrivateMethods
    {
        internal static bool Validate()
        {
            if (String.IsNullOrEmpty(CB.CloudApp.AppID) || String.IsNullOrEmpty(CB.CloudApp.AppKey))
            {
                throw new Exception.CloudBoostException("AppID / AppKey is missing.");
            }

            if (String.IsNullOrEmpty(CB.CloudApp.ApiUrl))
            {
                throw new Exception.CloudBoostException("API URL is missing.");
            }

            return true;
        }

        public static bool ColumnValidation(Column column, CloudTable cloudtable)
        {
            List<String> defaultColumns = new List<String>();
            defaultColumns.Add("id");
            defaultColumns.Add("createdAt");
            defaultColumns.Add("updatedAt");
            defaultColumns.Add("ACL");

            if(cloudtable.Type.ToLower() == "user")
            {
                defaultColumns.Add("username");
                defaultColumns.Add("email");
                defaultColumns.Add("password");
                defaultColumns.Add("roles");
                
            }else if(cloudtable.Type.ToLower() == "role"){
                defaultColumns.Add("name");
            }

            string colName = column.Name.ToLower();
            int index = defaultColumns.IndexOf(colName);
            
            if(index > -1)
                return false;
            
            return true;
        }

        public static bool TableValidation(string tableName)
        {
            char c = tableName.ElementAt(0);
            bool isDigit = (c >= '0' && c <= '9');
            if (isDigit)
            {
                return false;
            }

            if (tableName.Contains("^S+$"))
            {
                return false;
            }
            
            Regex pattern = new Regex("[~`!#$%\\^&*+=\\-\\[\\]\\';,/{}|\":<>\\?]");
            MatchCollection matches = pattern.Matches(tableName);

            if (matches.Count > 0)
            {
                return false;
            }

            return true;
        }

        

        public static bool TrimStart()
        {
            return true;
        }

        public static bool ColumnNameValidation(string columnName)
        {
            char c = columnName.ElementAt(0);
            bool isDigit = (c >= '0' && c <= '9');
            if (isDigit)
            {
                return false;
            }

            if (columnName.Contains("^S+$"))
            {
                return false;
            }

            Regex pattern = new Regex("[~`!#$%\\^&*+=\\-\\[\\]\\';,/{}|\":<>\\?]");
            MatchCollection matches = pattern.Matches(columnName);

            if (matches.Count > 0)
            {
                return false;
            }

            return true;
        }

        public static String _getSessionId()
        {
            //TODO: SQLLite for keeping session data
            String session = CloudApp.SESSION_ID;
            return session;
        }
        public static void _setSessionId(String session)
        {
            //TODO: SQLLite for keeping session data
            CloudApp.SESSION_ID = session;
        }
        public static void _deleteSessionId()
        {
            //TODO: SQLLite for keeping session data
            CloudApp.SESSION_ID = null;
        }

        public static void ColumnDataTypeValidation(string dataType)
        {
            List<string> dataTypeList = new List<string>();
            dataTypeList.Add("Text");
            dataTypeList.Add("Email");
            dataTypeList.Add("URL");
            dataTypeList.Add("Number");
            dataTypeList.Add("Boolean");
            dataTypeList.Add("GeoPoint");
            dataTypeList.Add("File");
            dataTypeList.Add("List");
            dataTypeList.Add("Relation");
            dataTypeList.Add("Object");
            dataTypeList.Add("EncryptedText");

            int index = dataTypeList.IndexOf(dataType);

            if (index < 0)
                throw new CB.Exception.CloudBoostException("Invalid data type");
       
        }
        
        public static bool FileCheck(CloudObject obj)
        {
            //SaveAsync call for each object of CloudObject Array 
            return true;
        }

        public static bool BulkFileCheck()
        {
            return true;
        }

        public static string GenerateHash()
        {
            return null;
        }

        internal static List<CloudObject> ToCloudObjectList(List<Dictionary<string, object>> result){

            List<CloudObject> objList = new List<CloudObject>();

            for(int i=0; i<result.Count; i++)
            {
                var obj = new CloudObject(result[i]["name"].ToString());
                obj.dictionary = result[i];
                objList.Add(obj);
            }

            return objList;
        }

        internal static List<CloudTable> ToCloudTableList(List<Dictionary<string, object>> result)
        {

            List<CloudTable> objList = new List<CloudTable>();

            for (int i = 0; i < result.Count; i++)
            {
                var obj = new CloudTable(result[i]["name"].ToString());
                obj.dictionary = result[i];
                objList.Add(obj);
            }

            return objList;
        }
    }
}
