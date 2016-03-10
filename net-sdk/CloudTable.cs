using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CB.Exception;
using System.Text.RegularExpressions;
using CB.Util;

namespace CB
{
    public class CloudTable
    {
        internal Dictionary<string, Object> dictionary = new Dictionary<string, Object>();

        public string Name
        {
            get { return dictionary["name"].ToString(); }
            set { dictionary["name"] = value; }
        }

        internal string Type
        {
            get { return dictionary["type"].ToString(); }
            set { dictionary["type"] = value; }
        }

        private int MaxCount
        {
            get
            {
                if (dictionary["maxCount"] != null)
                {
                    return Convert.ToInt32(dictionary["maxCount"]);
                }
                else
                {
                    return -1;
                }
            }
            set { dictionary["maxCount"] = value; }
        }

        public List<CB.Column> Columns
        {
            get
            {
                if (dictionary["columns"] != null)
                {
                    return (List<CB.Column>)dictionary["columns"];
                }
                else
                {
                    return new List<Column>();
                }
            }
            set { dictionary["columns"] = value; }
        }

        public CloudTable(string tableName)
        {  //new table constructor

            CB.PrivateMethods._tableValidation(tableName);
            this.Name = tableName;
            this.dictionary["appId"] = CB.CloudApp.AppID;

            if (tableName.ToLower() == "user")
            {
                this.Type = "user";
                this.MaxCount = 1;
            }
            else if (tableName.ToLower() == "role")
            {
                this.Type = "role";
                this.MaxCount = 1;
            }
            else
            {
                this.Type = "custom";
                this.MaxCount = 9999;
            }

            this.Columns = CB.CloudTable._defaultColumns(this.Type);
        }

        public void AddColumn(CB.Column column)
        {
            if (CB.Column._columnValidation(column, this))
                this.Columns.Add(column);
        }

        public void AddColumn(List<CB.Column> columns)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                if (CB.Column._columnValidation(columns[i], this))
                    this.Columns.Add(columns[i]);
            }
        }


        public void DeleteColumn(CB.Column column)
        {
            if (!column.IsDeleteable)
                throw new CloudBoostException(column.Name + " cannot be deleted.");

                if (CB.Column._columnValidation(column, this))
            {
                this.Columns.Remove(column);
            }

            
        }   

        public void DeleteColumn(string columnName)
        {
            var column = this.Columns.Where(o => o.Name == columnName).FirstOrDefault();

            if (column == null)
                throw new CB.Exception.CloudBoostException("Column with name " + columnName + " cannot be found.");

            if (CB.Column._columnValidation(column, this))
            {
                this.Columns.Remove(column);
            }
        }

        public void DeleteColumn(List<CB.Column> columns)
        {
            //yet to test
            for (var i = 0; i < columns.Count; i++)
            {
                if (CB.Column._columnValidation(columns[i], this))
                {
                    this.Columns.Remove(columns[i]);
                }
            }
        }

        public static async Task<List<CB.CloudTable>> GetAllAsync()
        {
            var result = await Util.CloudRequest.SendArray(Util.CloudRequest.Method.POST, CB.CloudApp.ApiUrl + "/app/" + CB.CloudApp.AppID + "/_getAll", null, true);

            List<CloudTable> tables = CB.PrivateMethods.ToCloudTableList(result);

            var tableDictionaries = (List<Dictionary<string, Object>>)result;

            return tables;
        }

        public static async Task<CB.CloudTable> GetAsync(string tableName)
        {
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, CB.CloudApp.ApiUrl + "/app/" + CB.CloudApp.AppID + "/" + tableName, null, true);
            CB.CloudTable table = new CloudTable(tableName);
            table.dictionary = (Dictionary<string, Object>)result;
            return table;
        }

        public static async Task<CB.CloudTable> GetAsync(CB.CloudTable table)
        {
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, CB.CloudApp.ApiUrl + "/app/" + CB.CloudApp.AppID + "/" + table, null, true);
            table.dictionary = (Dictionary<string, Object>)result;
            return table;
        }

        public async Task<CB.CloudTable> DeleteAsync()
        {
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, CB.CloudApp.ApiUrl + "/app/" + CB.CloudApp.AppID + "/" + this.dictionary["name"].ToString(), null, true);
            this.dictionary = (Dictionary<string, Object>)result;
            return (CB.CloudTable)this;
        }

        public async Task<CB.CloudTable> SaveAsync()
        {
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, CB.CloudApp.ApiUrl + "/" + CB.CloudApp.AppID + "/" + this.dictionary["name"].ToString(), null, true);
            this.dictionary = (Dictionary<string, Object>)result;
            return (CB.CloudTable)this;
        }

        internal static List<CB.Column> _defaultColumns(string tableType)
        {
            List<CB.Column> list = new List<Column>();

            var id = new CB.Column("id");
            id.DataType = DataType.Id;
            id.Required = true;
            id.Unique = true;
            id.IsDeleteable = false;
            id.IsEditable = false;

            var expires = new CB.Column("expires");
            expires.DataType = DataType.Number;
            expires.IsDeleteable = false;
            expires.IsEditable = false;


            var createdAt = new CB.Column("createdAt");
            createdAt.DataType = DataType.DateTime;
            createdAt.Required = true;
            createdAt.IsRenamable = false;
            createdAt.IsEditable = false;

            var updatedAt = new CB.Column("updatedAt");
            updatedAt.DataType = DataType.DateTime;
            updatedAt.Required = true;
            updatedAt.IsDeleteable = false;
            updatedAt.IsEditable = false;

            var ACL = new CB.Column("ACL");
            ACL.DataType = DataType.ACL;
            ACL.Required = true;
            ACL.IsDeleteable = false;
            ACL.IsEditable = false;

            list.Add(id);
            list.Add(ACL);
            list.Add(updatedAt);
            list.Add(createdAt);

            if (tableType == "user")
            {
                var username = new CB.Column("username");
                username.DataType = DataType.Text;
                username.Required = true;
                username.Unique = true;
                username.IsDeleteable = false;
                username.IsEditable = false;

                var email = new CB.Column("email");
                email.DataType =DataType.Email;
                email.Unique = true;
                email.IsDeleteable = false;
                email.IsEditable = false;

                var password = new CB.Column("password");
                password.DataType = DataType.EncryptedText;
                password.Required = true;
                password.IsDeleteable = false;
                password.IsEditable = false;

                var roles = new CB.Column("roles");
                roles.DataType = DataType.List;
                roles.RelatedTo = "Role";
                roles.IsDeleteable = false;
                roles.IsEditable = false;

                list.Add(username);
                list.Add(email);
                list.Add(password);

            }
            else if (tableType == "role")
            {
                var name = new CB.Column("name");
                name.DataType = DataType.Text;
                name.Unique = true;
                name.Required = true;
                name.IsDeleteable = false;
                name.IsDeleteable = false;
                list.Add(name);
            }

            return list;
        }
    }
}



