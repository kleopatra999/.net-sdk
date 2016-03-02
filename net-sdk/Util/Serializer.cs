using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB.Util
{
    class Serializer
    {
        internal static JObject Serialize(Object value)
        {
            throw new System.Exception();
        }

        internal static Dictionary<string,Object> Deserialize(JObject value)
        {
            return null;
        }

        internal static List<Dictionary<string, Object>> DeserializeArrayType(JObject value)
        {
            return null;
        }
    }
}
