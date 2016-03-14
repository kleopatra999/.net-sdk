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

            //CB.PrivateMethods._tableValidation(tableName);
            this.Name = tableName;
            this.dictionary["appId"] = CB.CloudApp.AppID;
            this.dictionary["_type"] = "table";
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
            //if (CB.Column._columnValidation(column, this))
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
            if (!column.isDeletable)
                throw new CloudBoostException(column.name + " cannot be deleted.");

                if (CB.Column._columnValidation(column, this))
            {
                this.Columns.Remove(column);
            }

            
        }   

        public void DeleteColumn(string columnName)
        {
            var column = this.Columns.Where(o => o.name == columnName).FirstOrDefault();

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
            Dictionary<string, Object> postData = new Dictionary<string, Object>();
            postData.Add("data", this.dictionary);
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.PUT, CB.CloudApp.ApiUrl + "/app/" + CB.CloudApp.AppID + "/" + this.dictionary["name"].ToString(), postData, true);
            this.dictionary = (Dictionary<string, Object>)result;
            return (CB.CloudTable)this;
        }

        internal static List<CB.Column> _defaultColumns(string tableType)
        {
            List<CB.Column> list = new List<Column>();

            var id = new CB.Column("id");
            id.dataType = DataType.Id.ToString();
            id.required = true;
            id.unique = true;
            id.isDeletable = false;
            id.isEditable = false;

            var expires = new CB.Column("expires");
            expires.dataType = DataType.Number.ToString();
            expires.isDeletable = false;
            expires.isEditable = false;


            var createdAt = new CB.Column("createdAt");
            createdAt.dataType = DataType.DateTime.ToString();
            createdAt.required = true;
            createdAt.isRenamable = false;
            createdAt.isEditable = false;
            createdAt.isDeletable = false;

            var updatedAt = new CB.Column("updatedAt");
            updatedAt.dataType = DataType.DateTime.ToString();
            updatedAt.required = true;
            updatedAt.isDeletable = false;
            updatedAt.isEditable = false;

            var ACL = new CB.Column("ACL");
            ACL.dataType = DataType.ACL.ToString();
            ACL.required = true;
            ACL.isDeletable = false;
            ACL.isEditable = false;

            list.Add(id);
            list.Add(ACL);
            list.Add(updatedAt);
            list.Add(createdAt);

            if (tableType == "user")
            {
                var username = new CB.Column("username");
                username.dataType = DataType.Text.ToString();
                username.required = true;
                username.unique = true;
                username.isDeletable = false;
                username.isEditable = false;

                var email = new CB.Column("email");
                email.dataType =DataType.Email.ToString();
                email.unique = true;
                email.isDeletable = false;
                email.isEditable = false;

                var password = new CB.Column("password");
                password.dataType = DataType.EncryptedText.ToString();
                password.required = true;
                password.isDeletable = false;
                password.isEditable = false;

                var roles = new CB.Column("roles");
                roles.dataType = DataType.List.ToString();
                roles.relatedTo = "Role";
                roles.isDeletable = false;
                roles.isEditable = false;

                list.Add(username);
                list.Add(email);
                list.Add(password);

            }
            else if (tableType == "role")
            {
                var name = new CB.Column("name");
                name.dataType = DataType.Text.ToString();
                name.unique = true;
                name.required = true;
                name.isDeletable = false;
                name.isDeletable = false;
                list.Add(name);
            }

            return list;
        }
    }
}



