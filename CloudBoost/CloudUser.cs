using CB.Exception;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB
{
    public class CloudUser : CloudObject
    {

        public static CloudUser Current { get; set; } //Current loggged in user is stored here. 
        public CloudUser() : base("User")
        {
            this.dictionary["_type"] = "user";
            this.dictionary["_modifiedColumns"] = new ArrayList();
            ((ArrayList)this.dictionary["_modifiedColumns"]).Add("createdAt");
            ((ArrayList)this.dictionary["_modifiedColumns"]).Add("updatedAt");
            ((ArrayList)this.dictionary["_modifiedColumns"]).Add("ACL");
            ((ArrayList)this.dictionary["_modifiedColumns"]).Add("expires");
            dictionary.Add("_isModified", true);
        }

        public string Username
        {
            get
            {
                return (string)dictionary["username"];
            }
            set
            {
                if (value.GetType() == typeof(string))
                {
                    dictionary["username"] = value;
                    _IsModified(this, "username");
                }
                else
                    throw new Exception.CloudBoostException("Value is not of type string");
            }

        }


        public string Password
        {
            get
            {
                return (string)dictionary["password"];
            }
            set
            {
                if (value.GetType() == typeof(string))
                {
                    dictionary["password"] = value;
                    _IsModified(this, "password");
                }
                else
                    throw new Exception.CloudBoostException("Value is not of type string");
            }

        }

        public string Email
        {
            get
            {
                return (string)dictionary["email"];
            }
            set
            {
                if (value.GetType() == typeof(string))
                {
                    dictionary["email"] = value;
                    _IsModified(this, "email");
                }
                else
                    throw new Exception.CloudBoostException("Value is not of type string");
            }

        }

        public async Task<CloudUser> Signup()
        {
            if (this.dictionary["username"] == null)
            {
                throw new Exception.CloudBoostException("Username is not set.");
            }
            
            if (this.dictionary["password"] == null)
            {
                throw new Exception.CloudBoostException("Password is not set.");
            }

            if (this.dictionary["email"] == null)
            {
                throw new Exception.CloudBoostException("Email is not set.");
            }

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, CB.CloudApp.ApiUrl + "/user/"+CB.CloudApp.AppID+"/signup", postData, false);
            this.dictionary = (Dictionary<string, Object>)result;
            return this;
        }

        public async Task<CloudUser> Login()
        {
            if (this.dictionary["username"] == null)
            {
                throw new Exception.CloudBoostException("Username is not set.");
            }

            if (this.dictionary["password"] == null)
            {
                throw new Exception.CloudBoostException("Password is not set.");
            }

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, CB.CloudApp.ApiUrl + "/user/"+CB.CloudApp.AppID+"/login", postData, false);
            this.dictionary = (Dictionary<string, Object>)result;
            CloudUser.Current = this; //set this user as current logged in user.
            return this;
        }

        public async Task<CloudUser> Logout()
        {
            if (this.dictionary["username"] == null)
            {
                throw new Exception.CloudBoostException("Username is not set.");
            }

            if (this.dictionary["password"] == null)
            {
                throw new Exception.CloudBoostException("Password is not set.");
            }

            if (this.dictionary["email"] == null)
            {
                throw new Exception.CloudBoostException("Email is not set.");
            }

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, CB.CloudApp.ApiUrl + "/user/"+CB.CloudApp.AppID+"/login", postData, false);
            this.dictionary = (Dictionary<string, Object>)result;
            CloudUser.Current = null; //set this user as current logged in user.
            return this;
        }

        public bool IsInRole(CloudRole role)
        {
            if (role == null || role.GetType() != typeof(CloudRole))
            {
                throw new Exception.CloudBoostException("Role is null or its not of type CloudRole");
            }

            return (((ArrayList)(this.Get("roles"))).IndexOf(role.ID) >= 0);
        }

        public static async Task<object> ResetPassword(string email)
        {
            if (email == null)
            {
                throw new CB.Exception.CloudBoostException("Email is required");
            }

            var param = new Dictionary<string, Object>();
            param["email"] = email;

            string url = CB.CloudApp.ApiUrl + "/user/" + CB.CloudApp.AppID + "/resetPassword"; ;
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, param, false);
            return result;
        }

        public static async Task<object> ChangePassword(string oldPassword, string newPassword)
        {
            var param = new Dictionary<string, Object>();
            param["oldPassword"] = oldPassword;
            param["newPassword"] = newPassword;

            string url = CB.CloudApp.ApiUrl + "/user/" + CB.CloudApp.AppID + "/changePassword"; ;
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, param, false);
            return result;
        }

        public async Task<CloudUser> AddToRole(CloudRole role)
        {
            if (this.ID == null)
            {
                throw new CloudBoostException("Please save this user before you add it to the role");
            }

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("user", this);
            postData.Add("role", role);

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.PUT, CB.CloudApp.ApiUrl + "/user/"+CB.CloudApp.AppID+"/addToRole", postData, false);
            this.dictionary = (Dictionary<string, Object>)result;
            return this;

        }

        public async Task<CloudUser> RemoveFromRole(CloudRole role)
        {
            if (this.ID == null)
            {
                throw new CloudBoostException("Please save this user before you remove it from the role");
            }

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("user", this);
            postData.Add("role", role);

            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.PUT, CB.CloudApp.ApiUrl + "/user/"+CB.CloudApp.AppID+"/removeFromRole", postData, false);
            this.dictionary = (Dictionary<string, Object>)result;
            return this;
        }
    }
}

