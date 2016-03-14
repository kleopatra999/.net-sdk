using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CB
{
    public class Column
    {
        public string name { get; set; }
        public string dataType { get; set;}
       
        public bool required { get; set; }
        public bool unique { get; set; }
        public bool isRenamable { get; set; }
        public bool isDeletable { get; set; }
        public bool isEditable { get; set; }
        public string relatedTo { get; set; }
        public string relationType { get; set; }
        public string _type { get; set; }

        public Column(string columnName, string dataType, bool required, bool unique)
        {
            this.name = columnName;
            this._type = "column";
            if (String.IsNullOrWhiteSpace(columnName))
            {
                CB.Column._columnNameValidation(columnName);
                this.name = columnName;
            }

             this.dataType = CB.DataType.Text.ToString();
            
            this.required = required;
            this.unique = unique;
            this.relatedTo = null;
            this.relationType = null;
            this.isEditable = true;
            this.isDeletable = true;
            this.isRenamable = false;
        }

        public Column(string columnName)
        {
            this.name = columnName;
            this._type = "column";
            if (String.IsNullOrWhiteSpace(columnName))
            {
                CB.Column._columnNameValidation(columnName);
                this.name = columnName;
            }

            this.dataType = CB.DataType.Text.ToString();
            this.required = false;
            this.unique = false;
            this.relatedTo = null;
            this.relationType = null;
            this.isEditable = true;
            this.isEditable = true;
            this.isRenamable = false;
        }

        internal static bool _columnValidation(CB.Column column, CB.CloudTable cloudtable) 
        {
            var defaultColumn = new List<string>();
            defaultColumn.Add("id");
            defaultColumn.Add("_id");
            defaultColumn.Add("updatedAt");
            defaultColumn.Add("createdAt");
            defaultColumn.Add("ACL");
            defaultColumn.Add("expires");

            if (cloudtable.Type == "user")
            {
                defaultColumn.Add("username");
                defaultColumn.Add("password");
                defaultColumn.Add("email");
                defaultColumn.Add("roles");
            }
            else if (cloudtable.Type == "role")
            {
                defaultColumn.Add("name");
            }

            var index = defaultColumn.IndexOf(column.name.ToLower());
            if (index == -1)
                return true;
            else
                return false;
        }

        internal static void _columnNameValidation (string columnName)
        {
            var defaultColumn = new string[] { "id", "_id", "createdAt", "updatedAt", "ACL", "expires" };

            if (String.IsNullOrWhiteSpace(columnName)) //if table name is empty
                throw new CB.Exception.CloudBoostException("Column name cannot be empty");
            
            var index = Array.IndexOf(defaultColumn,columnName.ToLower());
            if (index >= 0)
                throw new CB.Exception.CloudBoostException("Column name already in use.");

            int temp; 
            if (int.TryParse(columnName[0].ToString(), out temp))
                throw new CB.Exception.CloudBoostException("Column should not start with a number.");

            if (columnName.Contains(" "))
                throw new CB.Exception.CloudBoostException("Column should not contain spaces.");

            var regexItem = new Regex("^[a-zA-Z0-9 ]*");
            if (regexItem.IsMatch(columnName))
                throw new CB.Exception.CloudBoostException("Column name should not contain special characters");
        }
    }
}



