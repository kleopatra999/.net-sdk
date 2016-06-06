using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB
{
    public class CloudRole : CloudObject
    {
        public CloudRole(string roleName) : base("Role")
        {
            this.dictionary["_type"] = "role";
            this.dictionary["name"] = roleName;
            this.dictionary["_modifiedColumns"] = new ArrayList();
            ((ArrayList)this.dictionary["_modifiedColumns"]).Add("createdAt");
            ((ArrayList)this.dictionary["_modifiedColumns"]).Add("updatedAt");
            ((ArrayList)this.dictionary["_modifiedColumns"]).Add("ACL");
            ((ArrayList)this.dictionary["_modifiedColumns"]).Add("expires");
            ((ArrayList)this.dictionary["_modifiedColumns"]).Add("name");
            dictionary.Add("_isModified", true);
        }

        public string Name
        {
            get
            {
                return (string)dictionary["name"];
            }
            set
            {
                if (value.GetType() == typeof(string))
                {
                    dictionary["name"] = value;
                    _IsModified(this, "name");
                }
                else
                    throw new Exception.CloudBoostException("Value is not of type string");
            }
        }

        public static async Task<CloudRole> GetRole(string roleName)
        {
            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, "/role/getRole/" + roleName, null);
            var role = new CloudRole(roleName);
            role.dictionary = result;
            return role;
        }
    }
}
