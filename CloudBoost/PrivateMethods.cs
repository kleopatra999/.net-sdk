using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CB
{
    class PrivateMethods
    {
       

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

        internal static void _TableValidation(string tableName)
        {
            if (String.IsNullOrWhiteSpace(tableName)) //if table name is empty
                throw new CB.Exception.CloudBoostException("Table name cannot be empty");

            int temp; //JUNK

            if (int.TryParse(tableName[0].ToString(), out temp))
                throw new CB.Exception.CloudBoostException("Table name cannot start with a number");

            if (tableName.Contains(" "))
                throw new CB.Exception.CloudBoostException("Table name should not contain spaces");

            var regexItem = new Regex("^[a-zA-Z0-9 ]*");
            if (regexItem.IsMatch(tableName))
                throw new CB.Exception.CloudBoostException("Table name should not contain special characters");
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
            String session = CloudApp.Session;
            return session;
        }
        public static void _setSessionId(String session)
        {
            CloudApp.Session = session;
        }
        public static void _deleteSessionId()
        {
            CloudApp.Session = null;
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
    }
}
