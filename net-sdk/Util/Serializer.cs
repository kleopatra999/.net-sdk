using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace CB.Util
{
    class Serializer
    {
        internal static string Serialize(Dictionary<string, Object> data)
        {
            if (data == null)
            {
                return null;

            }
            string response = JsonConvert.SerializeObject(data);

            return response;
        }


        internal static Dictionary<string,Object> Deserialize(string response)
        {
            Dictionary<string, Object> obj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(response);
            return obj;
        }

        internal static List<Dictionary<string, Object>> DeserializeArrayType(string response)
        {
            List<Dictionary<string, Object>> obj = JsonConvert.DeserializeObject<List<Dictionary<string, Object>>>(response);
            return obj;
        }

        
    }
}
