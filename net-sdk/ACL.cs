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
            if (value)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"] = new ArrayList();
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).Add("all");
            }
            else
            {
                var index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).Remove("all");
                }
            }
        }
        public void SetPublicReadAccess(bool value)
        {   //for setting the public read access
            if (value)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"] = new ArrayList();
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).Add("all");
            }
            else
            {
                var index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).Remove("all");
                }
            }
        }
        public void SetUserWriteAccess(string userId, bool value)
        { //for setting the user write access
            if (value) { //If asked to allow user write access
                //remove public write access.
                var index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).Remove("all");
                }

                index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).IndexOf(userId);
                if (index <= -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).Add(userId);
                }
                
            } else {
                var index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).IndexOf(userId);
                if (index > -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).Remove(userId);
                }
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["deny"])["user"]).Add(userId);
                
            }
        }
        public void SetUserReadAccess(string userId, bool value)
        {
            if (value)
            { //If asked to allow user write access
                //remove public write access.
                var index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).Remove("all");
                }

                index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).IndexOf(userId);
                if (index <= -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).Add(userId);
                }

            }
            else
            {
                var index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).IndexOf(userId);
                if (index > -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).Remove(userId);
                }
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["deny"])["user"]).Add(userId);

            }
        }
        public void SetRoleWriteAccess(string roleId, bool value)
        { //for setting the user write access

            if (value) {
                //remove public write access.
                var index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).Remove("all");
                }
                index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["role"]).IndexOf(roleId);
                if (index <= -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["role"]).Add(roleId);
                }
            } else {
                var index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["role"]).IndexOf(roleId);
                if (index > -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["role"]).Remove("all");
                }

                index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["allow"])["user"]).Remove("all");
                }
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["write"])["deny"])["role"]).IndexOf(roleId);
           
            }
        }
        public void SetRoleReadAccess(string roleId, bool value)
        { //for setting the user read access
            if (value)
            {
                //remove public write access.
                var index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).Remove("all");
                }
                index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["role"]).IndexOf(roleId);
                if (index <= -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["role"]).Add(roleId);
                }
            }
            else
            {
                var index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["role"]).IndexOf(roleId);
                if (index > -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["role"]).Remove("all");
                }

                index = ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).IndexOf("all");
                if (index > -1)
                {
                    ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["allow"])["user"]).Remove("all");
                }
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["read"])["deny"])["role"]).IndexOf(roleId);

            }
        }
    }
}
