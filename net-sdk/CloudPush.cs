using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using System.Xml;
using System.Runtime;

namespace CB
{
    public static class CloudPush
    {
        internal static Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
        private const string ChannelUriKey = "";
        private const string ChannelUriDefault = null;
        private static string _channelUri;
        private static CB.CloudQuery query;
        private static PushNotificationChannel _channel;

        public static ArrayList Channel
        {
            get
            {
                return ((ArrayList)dictionary["channel"]);
            }
            set
            {
               dictionary["channel"] = value;
            }
        }

        public static Dictionary<string, object> Message
        {
            get
            {
                return (Dictionary<string, object>)dictionary["message"];
            }
            set
            {
                dictionary["message"] = value;
            }
        }


        public static string Type
        {
            get
            {
                return dictionary["type"].ToString();
            }
            set
            {
                dictionary["type"] = value;
            }
        }

        public static CB.CloudQuery Query
        {
            get
            {
                return query;
            }
            set
            {
                query = value;
            }
        }

        public static async void SendAsync()
        {           
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("data", Serialize(dictionary));
            postData.Add("query", query.Query);
            postData.Add("sort", query.Sort);
            postData.Add("limit", query.Limit);
            postData.Add("skip", query.Skip);
            string url = CB.CloudApp.ApiUrl + "/push/" + CB.CloudApp.AppID + "/send";
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, false);
            DeSerialize(result);
        }

        public async static void SubscribeAsync(ArrayList list)
        {
            dictionary["channelList"] = list;
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", Serialize(dictionary));
            string url = CB.CloudApp.ApiUrl + "/push/" + CB.CloudApp.AppID + "/subscribe/" + dictionary["registrationId"];
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, false);
            DeSerialize(result);
        }

        public async static Task<ArrayList> SubscribedChannelListAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", Serialize(dictionary));
            string url = CB.CloudApp.ApiUrl + "/push/" + CB.CloudApp.AppID + "/subscribe/list/" + dictionary["registrationId"];
            ArrayList result = (ArrayList)await Util.CloudRequest.SendObject(Util.CloudRequest.Method.POST, url, postData, false);
            return result;
        }

        public async static void UnsubscribeAsync(ArrayList list)
        {
            dictionary["channelList"] = list;
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", Serialize(dictionary));
            string url = CB.CloudApp.ApiUrl + "/push/" + CB.CloudApp.AppID + "/unsubscribe/" + dictionary["registrationId"];
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, false);
            DeSerialize(result);
        }

        public async static Task RegisterForPushNotification(string deviceUri, string appname)
        {
            TimeZone timezone = TimeZone.CurrentTimeZone;
            var obj = new CB.CloudObject("Device");
            obj.Set("deviceToken", deviceUri);
            obj.Set("deviceOS", "windows");
            obj.Set("timezone", timezone);
            obj.Set("channels", Channel);
            await obj.SaveAsync();
        }


        public static void PushService()
        {
            _channelUri = LocalSettingsLoad(ApplicationData.Current.LocalSettings, ChannelUriKey, ChannelUriDefault);
        }

        public static string ChannelUri
        {
            get 
            {
                return _channelUri;
            }
            private set
            {
                if (_channelUri != value)
                {
                    _channelUri = value;
                    LocalSettingsStore(ApplicationData.Current.LocalSettings, ChannelUriKey, value);
                }
            }
        }

        
        public static void PushNotificationReceived(PushCallback callback)
        {
           _channel.PushNotificationReceived += (PushNotificationChannel sender, PushNotificationReceivedEventArgs args) =>
            {
                callback(sender, args);
            };
        }

        public static async Task<string> InitAsync()
        {
            var retries = 3;
            var difference = 10; // In seconds
            var currentRetry = 0;

            do
            {
                try
                {
                    _channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                    //_channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                    //_channel.PushNotificationReceived += OnPushNotificationReceived;
                    if (!_channel.Uri.Equals(ChannelUri))
                    {
                        ChannelUri = _channel.Uri;
                        //register uri to PN Service
                        await RegisterForPushNotification(ChannelUri, "sdkapp");

                        RaiseChannelUriUpdated();
                        return _channel.Uri;
                    }
                }
                catch
                {
                    throw new CB.Exception.CloudBoostException("Could not create channel uri");
                }

                await Task.Delay(TimeSpan.FromSeconds(difference));
            } while (currentRetry++ < retries);

            return null;
        }

        private static void OnPushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            switch (args.NotificationType)
            {
                case PushNotificationType.Badge:
                    OnBadgeNotificationReceived(args.BadgeNotification.Content.GetXml());
                    break;

                case PushNotificationType.Tile:
                    OnTileNotificationReceived(args.TileNotification.Content.GetXml());
                    break;

                case PushNotificationType.Toast:
                    OnToastNotificationReceived(args.ToastNotification.Content.GetXml());
                    break;

                case PushNotificationType.Raw:
                    OnRawNotificationReceived(args.RawNotification.Content);
                    break;
            }

            args.Cancel = true;
        }

        public static void OnBadgeNotificationReceived(string notificationContent)
        {
            // Code when a badge notification is received when app is running
        }

        public static void OnTileNotificationReceived(string notificationContent)
        {
            // Code when a tile notification is received when app is running
        }

        public static void OnToastNotificationReceived(string notificationContent)
        {
            // Code when a toast notification is received when app is running

            // Show a toast notification programatically

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(notificationContent);
            //var toastNotification = new ToastNotification(xmlDocument);

            //toastNotification.SuppressPopup = true;
            //ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }

        private static void OnRawNotificationReceived(string notificationContent)
        {
            // Code when a raw notification is received when app is running
        }

        public static event EventHandler<EventArgs> ChannelUriUpdated;
        private static void RaiseChannelUriUpdated()
        {
            if (ChannelUriUpdated != null)
            {
                ChannelUriUpdated(null, new EventArgs());
            }
        }

        public static T LocalSettingsLoad<T>(ApplicationDataContainer settings, string key, T defaultValue)
        {
            T value;

            if (settings.Values.ContainsKey(key))
            {
                value = (T)settings.Values[key];
            }
            else
            {
                // Otherwise use the default value.
                value = defaultValue;
            }

            return value;
        }

        public static bool LocalSettingsStore(ApplicationDataContainer settings, string key, object value)
        {
            bool valueChanged = false;

            if (settings.Values.ContainsKey(key))
            {
                // If the key exists
                if (settings.Values[key] != value)
                {
                    // If the value has changed, store the new value
                    settings.Values[key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                // Otherwise create the key
                settings.Values.Add(key, value);
                valueChanged = true;
            }

            return valueChanged;
        }

        //Serialize
        private static Dictionary<string, Object> Serialize(Dictionary<string, Object> data)
        {
            Dictionary<string, Object> dic = new Dictionary<string, object>();

            foreach (var param in data)
            {
                if (param.Key == "expires")
                {
                    dic["expires"] = null;
                }
                else
                {
                    dic[param.Key] = param.Value;
                }

            }
            return dic;
        }

        private static void DeSerialize(Dictionary<string, Object> data)
        {
            Dictionary<string, Object> dic = new Dictionary<string, object>();

            foreach (var param in data)
            {
                if (param.Key == "expires")
                {

                }
                else
                {
                    dictionary[param.Key] = param.Value;
                }

            }
        }
    }
}
