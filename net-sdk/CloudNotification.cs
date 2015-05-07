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

        private static Socket _socket = IO.Socket(CloudApp.ServerUrl);

        public static void On(string channelName, Callback callback)
        {
            _socket.Emit("join-custom-channel", CloudApp.AppID + channelName);

            _socket.On(CloudApp.AppID + channelName, (data) =>
            {
                callback(data);
            });
        }

        public static void Off(string channelName)
        {
            _socket.Emit("leave-custom-channel", CloudApp.AppID + channelName);

            // TODO :Remove all listeners.
            //CB.Socket.removeAllListeners(CB.appId + channelName);
        }

        public static void Publsh(string channelName, Object data)
        {
            //TODO : :)
            // _socket.Emit("publish-custom-channel",{ channel: CB.appId + channelName,data: data});
        }

    }
}









