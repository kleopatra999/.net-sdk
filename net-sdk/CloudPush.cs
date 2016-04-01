using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.Foundation;
using System.Xml;

namespace CB
{
    class CloudPush
    {
        internal Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
        private const string ChannelUriKey = "";
        private const string ChannelUriDefault = null;
        private string _channelUri;
        private PushNotificationChannel _channel;
       

        public ArrayList Channel
        {
            get
            {
                return ((ArrayList)this.dictionary["channel"]);
            }
            set
            {
                this.dictionary["channel"] = value;
            }
        }

        public string Message
        {
            get
            {
                return this.dictionary["message"].ToString();
            }
            set
            {
                this.dictionary["message"] = value;
            }
        }

        public async Task<CB.CloudPush> SendAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", Serialize(this.dictionary));
            string url = CB.CloudApp.ApiUrl + "/push/" + CB.CloudApp.AppID + "/send";
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, false);
            return DeSerialize(result, this);

        }

        public async Task<CB.CloudPush> SubscribeAsync(ArrayList list)
        {
            this.dictionary["channelList"] = list;
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", Serialize(this.dictionary));
            string url = CB.CloudApp.ApiUrl + "/push/" + CB.CloudApp.AppID + "/subscribe/" + this.dictionary["registrationId"];
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, false);
            return DeSerialize(result, this);
        }

        public async Task<ArrayList> SubscribedChannelListAsync()
        {
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", Serialize(this.dictionary));
            string url = CB.CloudApp.ApiUrl + "/push/" + CB.CloudApp.AppID + "/subscribe/list/" + this.dictionary["registrationId"];
            ArrayList result = (ArrayList)await Util.CloudRequest.SendObject(Util.CloudRequest.Method.POST, url, postData, false);
            return result;
        }

        public async Task<CB.CloudPush> UnsubscribeAsync(ArrayList list)
        {
            this.dictionary["channelList"] = list;
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", Serialize(this.dictionary));
            string url = CB.CloudApp.ApiUrl + "/push/" + CB.CloudApp.AppID + "/unsubscribe/" + this.dictionary["registrationId"];
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, false);
            return DeSerialize(result, this);
        }

        private async Task<CB.CloudPush> registerForPN(string deviceUri)
        {
            TimeZone timezone = TimeZone.CurrentTimeZone;
            this.dictionary["timezone"] = timezone;
            this.dictionary["deviceType"] = "windows";
            this.dictionary["deviceId"] = null;
            this.dictionary["deviceUri"] = deviceUri;
            Dictionary<string, Object> postData = new Dictionary<string, object>();
            postData.Add("document", Serialize(this.dictionary));
            string url = CB.CloudApp.ApiUrl + "/push/" + CB.CloudApp.AppID + "/updateDevice";
            var result = await Util.CloudRequest.Send(Util.CloudRequest.Method.POST, url, postData, false);
            return DeSerialize(result, this);
        }


        public void PushService()
        {
            this._channelUri = LocalSettingsLoad(ApplicationData.Current.LocalSettings, ChannelUriKey, ChannelUriDefault);
        }

        public string ChannelUri
        {
            get 
            {
                return _channelUri;
            }
            private set
            {
                if (_channelUri != value)
                {
                    this._channelUri = value;
                    LocalSettingsStore(ApplicationData.Current.LocalSettings, ChannelUriKey, value);
                }
            }
        }

        
        public void CBPushNotificationReceived(PushCallback callback)
        {
         
            
           _channel.PushNotificationReceived += (PushNotificationChannel sender, PushNotificationReceivedEventArgs args) =>
            {
                callback(sender, args);
            };
        }

        public async Task<string> UpdateChannelUri()
        {
            var retries = 3;
            var difference = 10; // In seconds
            var currentRetry = 0;

            do
            {
                try
                {
                    _channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                    //_channel.PushNotificationReceived += OnPushNotificationReceived;
                    if (!_channel.Uri.Equals(ChannelUri))
                    {
                        ChannelUri = _channel.Uri;
                        //register uri to PN Service
                        await this.registerForPN(ChannelUri);

                        this.RaiseChannelUriUpdated();
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

        private void OnPushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            switch (args.NotificationType)
            {
                case PushNotificationType.Badge:
                    this.OnBadgeNotificationReceived(args.BadgeNotification.Content.GetXml());
                    break;

                case PushNotificationType.Tile:
                    this.OnTileNotificationReceived(args.TileNotification.Content.GetXml());
                    break;

                case PushNotificationType.Toast:
                    this.OnToastNotificationReceived(args.ToastNotification.Content.GetXml());
                    break;

                case PushNotificationType.Raw:
                    this.OnRawNotificationReceived(args.RawNotification.Content);
                    break;
            }

            args.Cancel = true;
        }

        public void OnBadgeNotificationReceived(string notificationContent)
        {
            // Code when a badge notification is received when app is running
        }

        public void OnTileNotificationReceived(string notificationContent)
        {
            // Code when a tile notification is received when app is running
        }

        public void OnToastNotificationReceived(string notificationContent)
        {
            // Code when a toast notification is received when app is running

            // Show a toast notification programatically

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(notificationContent);
            //var toastNotification = new ToastNotification(xmlDocument);

            //toastNotification.SuppressPopup = true;
            //ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }

        private void OnRawNotificationReceived(string notificationContent)
        {
            // Code when a raw notification is received when app is running
        }

        public event EventHandler<EventArgs> ChannelUriUpdated;
        private void RaiseChannelUriUpdated()
        {
            if (ChannelUriUpdated != null)
            {
                ChannelUriUpdated(this, new EventArgs());
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
        protected static Dictionary<string, Object> Serialize(Dictionary<string, Object> data)
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

        internal static CB.CloudPush DeSerialize(Dictionary<string, Object> data, CB.CloudPush obj)
        {
            Dictionary<string, Object> dic = new Dictionary<string, object>();

            foreach (var param in data)
            {
                if (param.Key == "expires")
                {

                }
                else
                {
                    obj.dictionary[param.Key] = param.Value;
                }

            }

            return obj;
        }
    }
}
