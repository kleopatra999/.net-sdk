using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CB.Exception;


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
            if (!column.IsDeletable)
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

        public CB.Column GetColumn(string columnName)
        {
            List<CB.Column> columns = this.Columns;
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].Name == columnName)
                {
                    return columns[i];
                }
            }

            throw new CB.Exception.CloudBoostException("Column Does Not Exists");
        }

        public CB.Column UpdateColumn(CB.Column column)
        {
            List<CB.Column> columns = this.Columns;

            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].Name == column.Name)
                {
                    columns[i] = column;
                }

                this.Columns = columns;
            }

            throw new CB.Exception.CloudBoostException("Invalid Column");
        }

        public static async Task<List<CB.CloudTable>> GetAllAsync()
        {
            var result = await Util.CloudRequest.Send<List<Dictionary<string, Object>>>(Util.CloudRequest.Method.POST, CB.CloudApp.ApiUrl + "/app/" + CB.CloudApp.AppID + "/_getAll", null);

            List<CloudTable> tables = CB.PrivateMethods.ToCloudTableList(result);

            var tableDictionaries = result;

            return tables;
        }

        public static async Task<CB.CloudTable> GetAsync(string tableName)
        {
            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, CB.CloudApp.ApiUrl + "/app/" + CB.CloudApp.AppID + "/" + tableName, null);
            CB.CloudTable table = new CloudTable(tableName);
            table.dictionary = result;
            return table;
        }

        public static async Task<CB.CloudTable> GetAsync(CB.CloudTable table)
        {
            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, CB.CloudApp.ApiUrl + "/app/" + CB.CloudApp.AppID + "/" + table, null);
            table.dictionary = result;
            return table;
        }

        public async Task<CB.CloudTable> DeleteAsync()
        {
            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, CB.CloudApp.ApiUrl + "/app/" + CB.CloudApp.AppID + "/" + this.dictionary["name"].ToString(), null);
            this.dictionary = result;
            return (CB.CloudTable)this;
        }

        public async Task<CB.CloudTable> SaveAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, Object>();
            postData.Add("data", this.dictionary);
            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.PUT, CB.CloudApp.ApiUrl + "/app/" + CB.CloudApp.AppID + "/" + this.dictionary["name"].ToString(), postData);
            this.dictionary = result;
            return (CB.CloudTable)this;
        }

        internal static List<CB.Column> _defaultColumns(string tableType)
        {
            List<CB.Column> list = new List<Column>();

            var id = new CB.Column("id");
            id.DataType = DataType.Id.ToString();
            id.Required = true;
            id.Unique = true;
            id.IsDeletable = false;
            id.IsEditable = false;

            var expires = new CB.Column("expires");
            expires.DataType = DataType.Number.ToString();
            expires.IsDeletable = false;
            expires.IsEditable = false;


            var createdAt = new CB.Column("createdAt");
            createdAt.DataType = DataType.DateTime.ToString();
            createdAt.Required = true;
            createdAt.IsRenamable = false;
            createdAt.IsEditable = false;
            createdAt.IsDeletable = false;

            var updatedAt = new CB.Column("updatedAt");
            updatedAt.DataType = DataType.DateTime.ToString();
            updatedAt.Required = true;
            updatedAt.IsDeletable = false;
            updatedAt.IsEditable = false;

            var ACL = new CB.Column("ACL");
            ACL.DataType = DataType.ACL.ToString();
            ACL.Required = true;
            ACL.IsDeletable = false;
            ACL.IsEditable = false;

            list.Add(id);
            list.Add(ACL);
            list.Add(updatedAt);
            list.Add(createdAt);

            if (tableType == "user")
            {
                var username = new CB.Column("username");
                username.DataType = DataType.Text.ToString();
                username.Required = true;
                username.Unique = true;
                username.IsDeletable = false;
                username.IsEditable = false;

                var email = new CB.Column("email");
                email.DataType =DataType.Email.ToString();
                email.Unique = true;
                email.IsDeletable = false;
                email.IsEditable = false;

                var password = new CB.Column("password");
                password.DataType = DataType.EncryptedText.ToString();
                password.Required = true;
                password.IsDeletable = false;
                password.IsEditable = false;

                var roles = new CB.Column("roles");
                roles.DataType = DataType.List.ToString();
                roles.RelatedTo = "Role";
                roles.IsDeletable = false;
                roles.IsEditable = false;

                list.Add(username);
                list.Add(email);
                list.Add(password);

            }
            else if (tableType == "role")
            {
                var name = new CB.Column("name");
                name.DataType = DataType.Text.ToString();
                name.Unique = true;
                name.Required = true;
                name.IsDeletable = false;
                name.IsDeletable = false;
                list.Add(name);
            }

            return list;
        }
    }
}



