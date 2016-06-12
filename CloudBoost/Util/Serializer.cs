using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Collections;

namespace CB.Util
{
    class Serializer
    {
        internal static object Serialize(object data)
        {
            if (data.GetType() == typeof(ArrayList))
            {
                ArrayList list = new ArrayList();

                for (int i = 0; i < ((ArrayList)data).Count; i++)
                {
                    list.Add(Serialize((((ArrayList)data)[i])));
                }

                return list;
            }
            else if (data.GetType().IsArray)
            {

                ArrayList newList = new ArrayList(((IEnumerable)data).Cast<object>()
                                  .Select(x => Serialize(x))
                                  .ToList());
                return newList;
            }
            else
            {
                Dictionary<string, Object> dic = new Dictionary<string, object>();

                if (data.GetType() == typeof(CloudObject))
                {
                    data = ((CloudObject)data).dictionary;
                }
                else if (data.GetType() == typeof(CloudUser))
                {
                    data = ((CloudUser)data).dictionary;
                }
                else if (data.GetType() == typeof(CloudRole))
                {
                    data = ((CloudRole)data).dictionary;
                }
                else if (data.GetType() == typeof(CloudGeoPoint))
                {
                    data = ((CloudGeoPoint)data).dictionary;
                }
                else if (data.GetType() == typeof(ACL))
                {
                    data = ((ACL)data).dictionary;
                }
                else if (data.GetType() == typeof(Dictionary<string, object>))
                {
                 foreach (var param in (Dictionary<string, object>)data)
                    {
                        if (param.Value == null)
                        {
                            dic[param.Key] = null;
                        }
                        else if (param.Key == "ACL")
                        {
                            dic["ACL"] = ((CB.ACL)param.Value).dictionary;
                        }
                        else if (param.Key == "expires")
                        {
                            dic["expires"] = param.Value;
                        }
                        else if ((param.Value).GetType() == typeof(CB.CloudObject))
                        {
                            dic[param.Key] = Serialize(((CB.CloudObject)param.Value).dictionary);
                        }
                        else if ((param.Value).GetType() == typeof(CB.CloudUser))
                        {
                            dic[param.Key] = Serialize(((CB.CloudUser)param.Value).dictionary);
                        }
                        else if ((param.Value).GetType() == typeof(CB.CloudRole))
                        {
                            dic[param.Key] = Serialize(((CB.CloudRole)param.Value).dictionary);
                        }
                        else if ((param.Value).GetType() == typeof(CB.CloudGeoPoint))
                        {
                            dic[param.Key] = Serialize(((CB.CloudGeoPoint)param.Value).dictionary);
                        }
                        else if ((param.Value).GetType() == typeof(Dictionary<string, Object>))
                        {
                            dic[param.Key] = Serialize((Dictionary<string, Object>)param.Value);
                        }
                        else if ((param.Value).GetType() == typeof(ArrayList))
                        {
                            dic[param.Key] = Serialize(param.Value);
                        }
                        else if ((param.Value).GetType().IsArray)
                        {
                            dic[param.Key] = Serialize(param.Value);
                        }
                        else
                        {
                            dic[param.Key] = param.Value;
                        }

                    }

                    return dic;
                } 
            }
            return data;
        }


        internal static object Deserialize(object data)
        {
          
            if (data.GetType() == typeof(ArrayList))
            {
                ArrayList newList = new ArrayList();

                for (int i = 0; i < ((ArrayList)data).Count; i++)
                {
                    newList.Add(Deserialize((((ArrayList)data)[i])));
                }

                return newList;
            }
            else if (data.GetType().IsArray || data.GetType() == typeof(JArray))
            {

                ArrayList newList = new ArrayList(((IEnumerable)data).Cast<object>()
                                  .Select(x => Deserialize(x))
                                  .ToList());
                return newList;
            }
            else if (data.GetType() == typeof(Dictionary<string, Object>) || data.GetType() == typeof(JObject))
            {
                CB.CloudObject obj = null;

                Dictionary<string, Object> tempData = null;

                if (data.GetType() == typeof(JObject))
                {
                    tempData = ((JObject)data).ToObject<Dictionary<string, object>>();
                }
                else
                {
                    tempData = (Dictionary<string, Object>)data;
                }

                if (((Dictionary<string, Object>)(tempData)).ContainsKey("_type"))
                {
                    if (obj == null && ((Dictionary<string, Object>)(tempData))["_type"] != null && ((Dictionary<string, Object>)(tempData))["_type"].ToString() == "custom")
                    {
                        obj = new CloudObject(((Dictionary<string, Object>)(tempData))["_tableName"].ToString());
                    }

                    if (obj == null && ((Dictionary<string, Object>)(tempData))["_type"] != null && ((Dictionary<string, Object>)(tempData))["_type"].ToString() == "user")
                    {
                        obj = new CloudUser();
                    }

                    if (obj == null && ((Dictionary<string, Object>)(tempData))["_type"] != null && ((Dictionary<string, Object>)(tempData))["_type"].ToString() == "role")
                    {
                        obj = new CloudRole(((Dictionary<string, Object>)(tempData))["name"].ToString());
                    }

                    if (obj == null && ((Dictionary<string, Object>)(tempData))["_type"] != null && ((Dictionary<string, Object>)(tempData))["_type"].ToString() == "point")
                    {
                        //return geopoint dictionary.
                        CloudGeoPoint point = new CloudGeoPoint(Decimal.Parse(((Dictionary<string, Object>)(tempData))["longitude"].ToString()), Decimal.Parse(((Dictionary<string, Object>)(tempData))["latitude"].ToString()));
                        return point;
                    }

                    foreach (var param in ((Dictionary<string, Object>)(tempData)))
                    {
                        if (param.Value == null)
                        {
                            obj.dictionary[param.Key] = null;
                        }
                        else if (param.Key == "ACL")
                        {
                            var acl = new CB.ACL();
                            obj.dictionary[param.Key] = acl;
                        }
                        else if (param.Key == "expires")
                        {
                            obj.dictionary["expires"] = ((Dictionary<string, Object>)(tempData))["expires"];
                        }
                        else if (param.Value != null && typeof(JObject) == param.Value.GetType() && ((JObject)(param.Value)).ToObject<Dictionary<string, object>>().ContainsKey("_type") && ((JObject)(param.Value)).ToObject<Dictionary<string, object>>()["_type"].ToString() == "custom")
                        {
                            CB.CloudObject cbObj = new CB.CloudObject(((JObject)(param.Value)).ToObject<Dictionary<string, object>>()["_tableName"].ToString());
                            cbObj = (CloudObject)Deserialize(((JObject)(param.Value)).ToObject<Dictionary<string, object>>());
                            obj.dictionary[param.Key] = cbObj;
                        }
                        else if (param.Value != null && typeof(JObject) == param.Value.GetType() && ((JObject)(param.Value)).ToObject<Dictionary<string, object>>().ContainsKey("_type") && ((JObject)(param.Value)).ToObject<Dictionary<string, object>>()["_type"].ToString() == "user")
                        {
                            CB.CloudUser cbObj = new CB.CloudUser();
                            cbObj = (CloudUser)Deserialize(((JObject)(param.Value)).ToObject<Dictionary<string, object>>());
                            obj.dictionary[param.Key] = cbObj;
                        }
                        else if (param.Value != null && typeof(JObject) == param.Value.GetType() && ((JObject)(param.Value)).ToObject<Dictionary<string, object>>().ContainsKey("_type") && ((JObject)(param.Value)).ToObject<Dictionary<string, object>>()["_type"].ToString() == "custom")
                        {
                            CB.CloudRole cbObj = new CB.CloudRole(((JObject)(param.Value)).ToObject<Dictionary<string, object>>()["name"].ToString());
                            cbObj = (CloudRole)Deserialize(((JObject)(param.Value)).ToObject<Dictionary<string, object>>());
                            obj.dictionary[param.Key] = cbObj;
                        }
                        else if (param.Value != null && typeof(JObject) == param.Value.GetType() && ((JObject)(param.Value)).ToObject<Dictionary<string, object>>().ContainsKey("_type") && ((JObject)(param.Value)).ToObject<Dictionary<string, object>>()["_type"].ToString() == "point")
                        {
                            CB.CloudGeoPoint cbObj = new CB.CloudGeoPoint(Decimal.Parse(((JObject)(param.Value)).ToObject<Dictionary<string, object>>()["longitude"].ToString()), Decimal.Parse(((JObject)(param.Value)).ToObject<Dictionary<string, object>>()["latitude"].ToString()));
                            cbObj = (CloudGeoPoint)(Deserialize(((JObject)(param.Value)).ToObject<Dictionary<string, object>>()));
                            obj.dictionary[param.Key] = cbObj;
                        }
                        else if (param.Value.GetType() == typeof(ArrayList))
                        {
                            obj.dictionary[param.Key] = Deserialize(param.Value);
                        }
                        else if (param.Value.GetType().IsArray)
                        {
                            obj.dictionary[param.Key] = Deserialize(param.Value);
                        }
                        else if (param.Value.GetType() == typeof(JArray))
                        {
                            obj.dictionary[param.Key] = Deserialize(param.Value);
                        }
                        else
                        {
                            obj.dictionary[param.Key] = param.Value;
                        }
                    }

                    return obj;
                }
                else
                {
                    return tempData;
                }
            }

            return data;
        }

        //check if the type can be converted to another type. 
        private static bool CanConvert<T>(object data)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            return converter.IsValid(data);
        }
    }
}
