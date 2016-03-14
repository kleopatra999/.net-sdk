using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB
{
    public class ACL
    {

        internal Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
        public ACL()
        { 
            dictionary["read"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)dictionary["read"])["allow"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"] = new ArrayList();
            ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).Add("all");
            ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["role"] = new ArrayList();
            ((Dictionary<string, Object>)dictionary["read"])["deny"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["deny"])["user"] = new ArrayList();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["deny"])["role"] = new ArrayList();

            dictionary["write"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)dictionary["write"])["allow"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"] = new ArrayList();
            ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).Add("all");
            ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["role"] = new ArrayList();
            ((Dictionary<string, Object>)dictionary["write"])["deny"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["deny"])["user"] = new ArrayList();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["deny"])["role"] = new ArrayList();

        }
        public void SetPublicWriteAccess(bool value)
        { //for setting the public write access
            if (dictionary["write"] == null || value)
            {
                ArrayList writeList = new ArrayList();
                writeList.Add("all");
                dictionary["write"] = writeList; //if the "write" property does not exist, create one with default value
            }
           
            if(!value)
            {
                var index = ((ArrayList)(dictionary["write"])).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)(dictionary["write"])).Remove("all"); //remove the "all" value from the "write" array of "this" object
                }
            }
        }
        public void SetPublicReadAccess(bool value)
        {   //for setting the public read access
            if (dictionary["read"] == null || value)
            {
                ArrayList readList = new ArrayList();
                readList.Add("all");
                dictionary["read"] = readList; //if the "read" property does not exist, create one with default value
            }

            if (!value)
            {
                var index = ((ArrayList)(dictionary["read"])).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)(dictionary["read"])).Remove("all"); //remove the "all" value from the "read" array of "this" object
                }
            }
        }
        public void SetUserWriteAccess(string userId, bool value)
        { //for setting the user write access
            if (dictionary["write"] == null)
            {
                ArrayList readList = new ArrayList();
                readList.Add("all");
                dictionary["write"] = readList; //if the "write" property does not exist, create one with default value
            }

            if (value)
            { //If asked to allow user write access
              //remove public write access.

                var index = ((ArrayList)(dictionary["write"])).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)(dictionary["write"])).Remove("all"); //remove the "all" value from the "read" array of "this" object
                }

                if (((ArrayList)(dictionary["write"])).IndexOf(userId) == -1)
                {
                    ((ArrayList)(dictionary["write"])).Add(userId);
                }
            }
            else
            {
                if (((ArrayList)(dictionary["write"])).IndexOf(userId) > -1)
                {
                    ((ArrayList)(dictionary["write"])).Remove(userId);
                }
            }
        }
        public void SetUserReadAccess(string userId, bool value)
        { //for setting the user read access
            if (dictionary["read"] == null)
            {
                ArrayList readList = new ArrayList();
                readList.Add("all");
                dictionary["read"] = readList; //if the "read" property does not exist, create one with default value
            }

            if (value)
            { //If asked to allow user read access
              //remove public read access.

                var index = ((ArrayList)(dictionary["read"])).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)(dictionary["read"])).Remove("all"); //remove the "all" value from the "read" array of "this" object
                }

                if (((ArrayList)(dictionary["read"])).IndexOf(userId) == -1)
                {
                    ((ArrayList)(dictionary["read"])).Add(userId);
                }
            }
            else
            {
                if (((ArrayList)(dictionary["read"])).IndexOf(userId) > -1)
                {
                    ((ArrayList)(dictionary["read"])).Remove(userId);
                }
            }
        }
        public void SetRoleWriteAccess(string roleId, bool value)
        { //for setting the user write access
            if (dictionary["write"] == null)
            {
                ArrayList readList = new ArrayList();
                readList.Add("all");
                dictionary["write"] = readList; //if the "write" property does not exist, create one with default value
            }

            if (value)
            { //If asked to allow user write access
              //remove public write access.

                var index = ((ArrayList)(dictionary["write"])).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)(dictionary["write"])).Remove("all"); //remove the "all" value from the "read" array of "this" object
                }

                if (((ArrayList)(dictionary["write"])).IndexOf(roleId) == -1)
                {
                    ((ArrayList)(dictionary["write"])).Add(roleId);
                }
            }
            else
            {
                if (((ArrayList)(dictionary["write"])).IndexOf(roleId) > -1)
                {
                    ((ArrayList)(dictionary["write"])).Remove(roleId);
                }
            }
        }
        public void SetRoleReadAccess(string roleId, bool value)
        { //for setting the user read access
            if (dictionary["read"] == null)
            {
                ArrayList readList = new ArrayList();
                readList.Add("all");
                dictionary["read"] = readList; //if the "read" property does not exist, create one with default value
            }

            if (value)
            { //If asked to allow user read access
              //remove public read access.

                var index = ((ArrayList)(dictionary["read"])).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)(dictionary["read"])).Remove("all"); //remove the "all" value from the "read" array of "this" object
                }

                if (((ArrayList)(dictionary["read"])).IndexOf(roleId) == -1)
                {
                    ((ArrayList)(dictionary["read"])).Add(roleId);
                }
            }
            else
            {
                if (((ArrayList)(dictionary["read"])).IndexOf(roleId) > -1)
                {
                    ((ArrayList)(dictionary["read"])).Remove(roleId);
                }
            }
        }
    }
}
