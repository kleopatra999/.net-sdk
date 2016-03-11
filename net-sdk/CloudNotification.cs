using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quobject.SocketIoClientDotNet.Client;


namespace CB
{
    public class CloudNotification
    {

        private static Socket _socket = IO.Socket(CloudApp.ApiUrl);

        public static void On(string channelName, Callback callback)
        {
            CB.PrivateMethods.Validate();
            _socket.Emit("join-custom-channel", CloudApp.AppID + channelName);
            _socket.On(CloudApp.AppID + channelName, (data) =>
            {
                callback(data);
            });
        }

        public static void Off(string channelName)
        {
            CB.PrivateMethods.Validate();
            _socket.Emit("leave-custom-channel", CloudApp.AppID + channelName);
            _socket.Off(CB.CloudApp.AppID + channelName);
        }

        public static void Publsh(string channelName, Object data)
        {
            CB.PrivateMethods.Validate();
            Dictionary<string, object> jsonObj = new Dictionary<string, object>();
            jsonObj.Add("channel", CB.CloudApp.AppID);
            jsonObj.Add("data", data);
            _socket.Emit("publish-custom-channel", jsonObj);
        }

    }
}









