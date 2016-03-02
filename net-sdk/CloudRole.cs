using System;
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
                    dictionary["name"] = value;
                else
                    throw new Exception.CloudBoostException("Value is not of type string");
            }

        }

        public static async Task<CloudRole> GetRole(string roleName)
        {
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, "/role/getRole/" + roleName, null, false);
            var role = new CloudRole(roleName);
            role.dictionary = (Dictionary<string, Object>)result;
            return role;
        }
    }
}
