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
        public string Name { get; set; }
        public CB.DataType DataType { get; set; }
        public bool Required { get; set; }
        public bool Unique { get; set; }
        internal bool IsRenamable { get; set; }
        internal bool IsDeleteable { get; set; }
        internal bool IsEditable { get; set; }
        public string RelatedTo { get; set; }

        public Column(string columnName, CB.DataType dataType, bool required, bool unique)
        {
            if (String.IsNullOrWhiteSpace(columnName))
            {
                CB.Column._columnNameValidation(columnName);
                this.Name = columnName;
            }

            if (dataType != null)
            {
                this.DataType = dataType;
            }
            else
            {
                this.DataType = CB.DataType.Text;
            }

            this.Required = required;
            this.Unique = unique;
            
            this.IsDeleteable = true;
            this.IsEditable = true;
            this.IsRenamable = true;
        }

        public Column(string columnName)
        {
            if (String.IsNullOrWhiteSpace(columnName))
            {
                CB.Column._columnNameValidation(columnName);
                this.Name = columnName;
            }

            this.DataType = CB.DataType.Text;
            this.Required = false;
            this.Unique = false;

            this.IsDeleteable = true;
            this.IsEditable = true;
            this.IsRenamable = true;
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

            var index = defaultColumn.IndexOf(column.Name.ToLower());
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



