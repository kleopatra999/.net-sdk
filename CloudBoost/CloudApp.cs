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
    public delegate void PushCallback(Object data1, Object data2);

    public class CloudApp
    {

        //Local Storage Container.
        //private static Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

        private static string _session;

        private static string apiUrl;

        internal static ILog log;

        internal static Socket _socket = IO.Socket(CloudApp.ApiUrl);

        internal static string Session
        {
            get
            {
                //if (_session != null)
                    return _session;
                //else
                //{
                //    //read from local storage
                //    if(roamingSettings.Values["session"] !=null)
                //    {
                //        _session = roamingSettings.Values["session"].ToString();
                //        return _session;
                //    }
                //    else
                //    {
                //        return null;
                //    }
                //}
            }
            set
            {
                if (value == null)
                {
                    //roamingSettings.Values["session"] = null;
                    _session = null;
                }
                else
                {
                    //roamingSettings.Values["session"] = value;
                    _session = value;
                }
            }
        }

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

        public static string AppID { get; set; }
        public static string AppKey { get; set; }

        public static void Init(string appId, string appKey)
        {
            log = LogManager.GetLogger(typeof(CloudApp));
            AppID = appId;
            AppKey = appKey;
        }

        public static void Init(string apiUrl, string appId, string appKey)
        {
            if (apiUrl.EndsWith("/"))
            {
                apiUrl = apiUrl.TrimEnd('/');
            }

            ApiUrl = apiUrl;
            AppID = appId;
            AppKey = appKey;
        }

        
        public static void OnConnect()
        {
            _socket.On(Socket.EVENT_CONNECT, () =>
            {
                _socket.Emit("connected");

            });
		}	
		
	    public static void Connect()
        {
            _socket.Connect();
	    }

	    public static void Disconnect()
        {
            _socket.Disconnect();
	    }

	    public static void OnDisconnect()
        {
            _socket.On(Socket.EVENT_DISCONNECT, () =>
            {
                _socket.Emit("disconnected");
            });
	    }
    }
}
