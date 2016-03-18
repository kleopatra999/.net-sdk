using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quobject.SocketIoClientDotNet.Client;

namespace CB
{

    public delegate void Callback(Object result);

    public class CloudApp
    {
        internal static String SESSION_ID = null;

        private static string apiUrl;

        internal static ILog log;

        internal static Socket _socket = IO.Socket(CloudApp.ApiUrl);

        public static string ApiUrl
        {
            get
            {
                if (apiUrl != null)
                {
                    return apiUrl;
                }
                else
                {
                    return @"https://api.cloudboost.io";
                }
            }
            set { apiUrl = value; }
        }

        private static string serviceUrl;

        public static string ServiceURL
        {
            get
            {
                if (serviceUrl != null)
                {
                    return serviceUrl;
                }
                else
                {
                    return @"https://service.cloudboost.io";
                }
            }
            set { serviceUrl = value; }
        }

        public static string AppID { get; set; }
        public static string AppKey { get; set; }

        public static void init(string appId, string appKey)
        {
            log = LogManager.GetLogger(typeof(CloudApp));
            AppID = appId;
            AppKey = appKey;
        }

        public static void init(string apiUrl, string serviceUrl, string appId, string appKey)
        {
            if (apiUrl.EndsWith("/"))
            {
                apiUrl = apiUrl.TrimEnd('/');
            }

            ApiUrl = apiUrl;
            AppID = appId;
            AppKey = appKey;
        }

        
        public static void onConnect()
        {
            _socket.On(Socket.EVENT_CONNECT, () =>
            {
                _socket.Emit("connected");

            });
		}	
		
	    public static void connect()
        {
            _socket.Connect();
	    }

	    public static void disconnect()
        {
            _socket.Disconnect();
	    }

	    public static void onDisconnect()
        {
            _socket.On(Socket.EVENT_DISCONNECT, () =>
            {
                _socket.Emit("disconnected");

            });
	    }
    }
}
