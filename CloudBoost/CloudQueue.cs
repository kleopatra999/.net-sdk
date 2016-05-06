using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB
{
    public class CloudQueue
    {
        internal Dictionary<string, Object> dictionary = new Dictionary<string, object>();
        public CloudQueue(string queueName, string queueType = null)
        {
            dictionary.Add("ACL", new CB.ACL());
            dictionary.Add("_type", "queue");
            dictionary.Add("expires", null);
            dictionary.Add("name", queueName);
            dictionary.Add("retry", null);
            dictionary.Add("subscribers", new List<Object>());
            dictionary.Add("messages", new List<Object>());

            if (queueType != "push" && queueType != "pull")
            {
                throw new CB.Exception.CloudBoostException("Type can be push or pull");
            }

            if (queueType != null)
            {
                dictionary.Add("queueType", queueType);
            }
            else
            {
                dictionary.Add("queueType", "pull");
            }
        }

        public CloudQueue(string queueName)
        {
            dictionary.Add("ACL", new CB.ACL());
            dictionary.Add("_type", "queue");
            dictionary.Add("expires", null);
            dictionary.Add("name", queueName);
            dictionary.Add("retry", null);
            dictionary.Add("subscribers", new List<Object>());
            dictionary.Add("messages", new List<Object>());
            dictionary.Add("queueType", "pull");
        }


        public string retry
        {
            get
            {
                return (string)dictionary["retry"];
            }
            set
            {
                if (dictionary["queueType"].ToString() != "push")
                    throw new CB.Exception.CloudBoostException("Queue Type should be push to set this property");

                dictionary["retry"] = retry;
                _IsModified(this, "retry");
            }
        }

        public int size
        {
            get
            {
                if (dictionary["size"] != null)
                {
                    return (int)dictionary["size"];
                }
                else
                {
                    return 0;
                }
            }
        }

        public string name
        {
            get
            {
                return (string)dictionary["name"];
            }
        }

        public List<Object> subscribers
        {
            get
            {
                return (List<Object>)dictionary["subscribers"];
            }
        }

        public string type
        {
            get
            {
                return (string)dictionary["queueType"];
            }
            set
            {
                dictionary["queueType"] = value;
                _IsModified(this, "queueType");
            }
        }

        public CB.ACL ACL
        {
            get
            {
                return (CB.ACL)dictionary["ACL"];
            }
            set
            {
                dictionary["ACL"] = value;
                _IsModified(this, "ACL");
            }
        }

        public string id
        {
            get
            {
                return (string)dictionary["_id"];
            }
        }

        public DateTime createdAt
        {
            get
            {
                return (DateTime)dictionary["createdAt"];
            }
        }

        public DateTime updatedAt
        {
            get
            {
                return (DateTime)dictionary["updatedAt"];
            }
        }

        public DateTime expires
        {
            get
            {
                return (DateTime)dictionary["expires"];
            }
        }

        public async Task<QueueMessage> addMessageAsync(List<Object> queueMessage)
        {
            List<Object> messages = new List<Object>();
            for (int i = 0; i < queueMessage.Count; i++)
            {
                messages.Add(queueMessage.ElementAt(i));
            }

            dictionary["messages"] = messages;

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var url = CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/message";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.PUT, url, postData);
            Dictionary<string, object> dic = (Dictionary<string, Object>)result;
            var qMessage = new CB.QueueMessage(dic["message"]);
            qMessage.dictionary = dic;
            return qMessage;
        }

        public async Task<CB.QueueMessage> addMessageAsync(string queueMessage)
        {
            List<Object> message = new List<Object>();
            message.Add(queueMessage);
            return await addMessageAsync(message);
        }

        public async Task<CB.QueueMessage> updateMessageAsync(List<Object> queueMessage)
        {
            List<Object> messages = new List<Object>();
            for (int i = 0; i < queueMessage.Count; i++)
            {
                //TODO: check for message id
                messages.Add(queueMessage.ElementAt(i));
            }

            return await this.addMessageAsync(queueMessage);

        }

        public async Task<CB.QueueMessage> updateMessageAsync(string queueMessage)
        {
            List<Object> message = new List<Object>();
            message.Add(queueMessage);
            return await updateMessageAsync(message);
        }

        public async Task<CB.QueueMessage> getMessageAsync(int count)
        {
            var thisObj = this;
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("count", count);

            var url = CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/getMessage";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);
            
            var qMessage = new CB.QueueMessage(result["message"]);
            qMessage.dictionary = result;
            return qMessage;
        }

        public async Task<CB.QueueMessage> getMessageAsync()
        {
            var thisObj = this;
            Dictionary<string, Object> postData = new Dictionary<string, object>();

            var url = CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/getMessage";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);
            
            var qMessage = new CB.QueueMessage(result["message"]);
            qMessage.dictionary = result;
            return qMessage;
        }

        public async Task<CloudQueue> getAllMessagesAsync()
        {
            var thisObj = this;
            Dictionary<string, Object> postData = new Dictionary<string, object>();
    
            var url = CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/message/" + id;

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);

            this.dictionary = result;

            return this;
        }

        public async Task<CB.QueueMessage> getMessageById(string id)
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();

            var url = CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/message" + id;

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);
            
            var qMessage = new CB.QueueMessage(result["message"]);
            qMessage.dictionary = result;
            return qMessage;
        }

        public async Task<CloudQueue> GetAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();

            var url = CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);

            this.dictionary = result;

            return this;
        }

        public async Task<CloudQueue> CreateAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();

            postData.Add("document", this);

            var url = CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/create";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);

            this.dictionary = result;

            return this;
        }
        
        public async Task<CloudQueue> AddSubscriberAsync(List<Object> url)
        {
            var tempSubscribers = dictionary["subscribers"];
            dictionary["subscribers"] = url;
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var Url =  CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/subscriber";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, Url, postData);

            this.dictionary = result;

            return this;
        }

        public async Task<CloudQueue> RemoveSubscriberAsync(List<Object> url)
        {
            var tempSubscribers = dictionary["subscribers"];
            dictionary["subscribers"] = url;
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var Url = CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/subscriber";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.DELETE, Url, postData);

            this.dictionary = result;

            return this;
        }

        public async Task<CB.QueueMessage> PeekMessageAsync(int count)
        {
            
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("count", count);

            var url = CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/peekMessage";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);
            
            var qMessage = new CB.QueueMessage(result["message"]);
            return qMessage;
        }

        public async Task<CB.QueueMessage> PeekMessageAsync()
        {

            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("count", null);

            var url = CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/peekMessage";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);

            var qMessage = new CB.QueueMessage(result["message"]);
            return qMessage;
        }
        
        public async Task<CloudQueue> DeleteAsync()
        {
            
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var url =  CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"];

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.DELETE, url, postData);

            this.dictionary = result;

            return this;
        }

        public async Task<CloudQueue> ClearAsync()
        {
            
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", this);

            var url =  CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/clear";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.DELETE, url, postData);

            this.dictionary = result;

            return this;
        }

        public async Task<CloudQueue> RefreshMessageTimeoutAsync(QueueMessage id, DateTime timeout)
        {
            if(id.id == null){
                throw new CB.Exception.CloudBoostException("Queue Message should have an id");
            }
            
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("timeout", timeout);

            var url =  CB.CloudApp.ApiUrl+ "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/" + id +"/refresh-message-timeout";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.PUT, url, postData);

            this.dictionary = result;

            return this;
        }

        public async Task<CB.QueueMessage> DeleteMessageAsync(object id)
        {
            string _id;
            if (id.GetType() == typeof(CB.QueueMessage))
            {
                _id = ((QueueMessage)id).id;
            }
            else
            {
                _id = id.ToString();
            }
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            
            var url =  CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/" + dictionary["name"] + "/message/"+ _id;

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.DELETE, url, postData);

            if (result == null)
            {
                return null;
            }
            
            var qMessage = new CB.QueueMessage(result["message"]);
            qMessage.dictionary = result;
            return qMessage;
        }
        
        public static async Task<CloudQueue> GetAllAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();

            var url = CB.CloudApp.ApiUrl + "/queue/" + CB.CloudApp.AppID + "/";

            var result = await Util.CloudRequest.Send<Dictionary<string, Object>>(Util.CloudRequest.Method.POST, url, postData);
            
            var queue = new CB.CloudQueue(result["name"].ToString(), null);

            return queue;
        }

        private static void _IsModified(CB.CloudQueue cbObj, string columnName)
        {
            cbObj.dictionary["_isModified"] = true;

            if (cbObj.dictionary["_modifiedColumns"] == null)
            {
                cbObj.dictionary["_modifiedColumns"] = new ArrayList();
                ((ArrayList)cbObj.dictionary["_modifiedColumns"]).Add(columnName);
            }
            else if (((ArrayList)cbObj.dictionary["_modifiedColumns"]).Contains(columnName) == false)
            {
                ((ArrayList)cbObj.dictionary["_modifiedColumns"]).Add(columnName);
            }

        }
    }

    public class QueueMessage
    {
        internal Dictionary<string, Object> dictionary = new Dictionary<string, object>();
        public QueueMessage(object data)
        {
            dictionary.Add("ACL", new ACL());
            dictionary.Add("_type", "queue-message");
            dictionary.Add("expires", null);
            dictionary.Add("timeout", 1800);
            dictionary.Add("delay", null);
            dictionary.Add("message", data);
            dictionary.Add("_id", null);
            List<string> modifiedColumns = new List<string>();
            modifiedColumns.Add("createdAt");
            modifiedColumns.Add("updatedAt");
            modifiedColumns.Add("ACL");
            modifiedColumns.Add("expires");
            modifiedColumns.Add("timeout");
            modifiedColumns.Add("delay");
            modifiedColumns.Add("message");
            dictionary.Add("_modifiedColumns", modifiedColumns);
            dictionary.Add("_isModified", true);
        }

        public QueueMessage()
        {
            dictionary.Add("ACL", new ACL());
            dictionary.Add("_type", "queue-message");
            dictionary.Add("expires", null);
            dictionary.Add("timeout", 1800);
            dictionary.Add("delay", null);
            dictionary.Add("message", null);
            dictionary.Add("_id", null);
            dictionary["_modifiedColumns"] = new ArrayList();
            ((ArrayList)dictionary["_modifiedColumns"]).Add("createdAt");
            ((ArrayList)dictionary["_modifiedColumns"]).Add("updatedAt");
            ((ArrayList)dictionary["_modifiedColumns"]).Add("ACL");
            ((ArrayList)dictionary["_modifiedColumns"]).Add("expires");
            ((ArrayList)dictionary["_modifiedColumns"]).Add("timeout");
            ((ArrayList)dictionary["_modifiedColumns"]).Add("delay");
            ((ArrayList)dictionary["_modifiedColumns"]).Add("message");
            dictionary.Add("_isModified", true);
        }

        public object message
        {
            get
            {
                return dictionary["message"];
            }
            set
            {
                dictionary["message"] = value;
                _IsModified(this, "message");
            }
        }

        public CB.ACL ACL
        {
            get
            {
                return (CB.ACL)dictionary["ACL"];
            }
            set
            {
                dictionary["ACL"] = value;
                _IsModified(this, "ACL");
            }

        }

        public string id
        {
            get
            {
                return (string)dictionary["_id"];
            }
        }

        public DateTime createdAt
        {
            get
            {
                return (DateTime)dictionary["createdAt"];
            }
            set
            {
                dictionary["createdAt"] = value;
                _IsModified(this, "createdAt");
            }
        }

        public DateTime updatedAt
        {
            get
            {
                return (DateTime)dictionary["updatedAt"];
            }
            set
            {
                dictionary["updatedAt"] = value;
                _IsModified(this, "updatedAt");
            }
        }

        public DateTime expires
        {
            get
            {
                return (DateTime)dictionary["expires"];
            }
            set
            {
                dictionary["expires"] = value;
                _IsModified(this, "expires");
            }
        }

        public DateTime timeout
        {
            get
            {
                return (DateTime)dictionary["timeout"];
            }
            set
            {
                dictionary["timeout"] = value;
                _IsModified(this, "timeout");
            }
        }

        public int delay
        {
            get
            {
                return (int)dictionary["delay"];
            }
            set
            {
                dictionary["delay"] = value;
                _IsModified(this, "delay");
            }
        }

        private static void _IsModified(CB.QueueMessage cbObj, string columnName)
        {
            cbObj.dictionary["_isModified"] = true;

            if (cbObj.dictionary["_modifiedColumns"] == null)
            {
                cbObj.dictionary["_modifiedColumns"] = new ArrayList();
                ((ArrayList)cbObj.dictionary["_modifiedColumns"]).Add(columnName);
            }
            else if (((ArrayList)cbObj.dictionary["_modifiedColumns"]).Contains(columnName) == false)
            {
                ((ArrayList)cbObj.dictionary["_modifiedColumns"]).Add(columnName);
            }

        }

        internal static bool Modified(CB.CloudQueue obj, string columnName)
        {

            List<Object> modifiedColumns = new List<Object>();
            List<Object> col = new List<Object>();
       
            try
            {
                col = obj.dictionary.Select(_modifiedColumns => _modifiedColumns.Value).ToList();
            }
            catch (IndexOutOfRangeException e2)
            {

                throw e2;
            }
            for (int i = 0; i < col.Count; i++)
            {
                try
                {
                    modifiedColumns.Add(col.ElementAt(i));
                }
                catch (IndexOutOfRangeException e)
                {

                    throw new IndexOutOfRangeException(e.Message);
                }
                catch (CB.Exception.CloudBoostException e)
                {

                    throw new CB.Exception.CloudBoostException(e.Message);
                }
            }
            try
            {
                obj.dictionary.Add("_isModified", true);
            }
            catch (CB.Exception.CloudBoostException e1)
            {

                throw new CB.Exception.CloudBoostException(e1.Message); ;
            }

            if (modifiedColumns.Contains(columnName))
            {
                modifiedColumns.Clear();
                modifiedColumns.Add(columnName);
            }
            else
            {
                modifiedColumns.Add(columnName);
            }
            try
            {
                obj.dictionary.Add("_modifiedColumns", modifiedColumns);
            }
            catch (IndexOutOfRangeException e)
            {

                throw new IndexOutOfRangeException(e.Message);
            }
            return true;
        }
    }
}
