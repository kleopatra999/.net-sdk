using System.Collections.Generic;
using Quobject.SocketIoClientDotNet.Client;
using System.Web.UI.WebControls.WebParts;
using Newtonsoft.Json;
using System.Text;

namespace CB
{
    public class CloudNotification
    {
        private static readonly Socket _socket = IO.Socket(CloudApp.ApiUrl);

        public static void On(string channelName, Callback callback)
        {
            Util.CloudRequest.Validate();
            _socket.Emit("join-custom-channel", CloudApp.AppID + channelName);
            _socket.On(CloudApp.AppID + channelName, data => { callback(data); });
        }

        public static void Off(string channelName)
        {
            Util.CloudRequest.Validate();
            _socket.Emit("leave-custom-channel", CloudApp.AppID + channelName);
            _socket.Off(CloudApp.AppID + channelName);
        }

        public static void Publish(string channelName, object data)
        {

            Util.CloudRequest.Validate();
            var jsonObj = new Dictionary<string, object>
            {
                { "channel", CloudApp.AppID + channelName },
                { "data", data }
            };
            var json = JsonConvert.SerializeObject(Util.Serializer.Serialize(jsonObj));
            var payload  = Encoding.UTF8.GetBytes(json.ToString());
            
            _socket.Emit("publish-custom-channel", payload);
        }
    }
}