using System;
using System.Configuration;
using System.Collections.Specialized;

using static NetGameShared.Util.Config;

namespace NetGameClient
{
    public class Config
    {
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public string ServerConnectionKey { get; set; }

        public Config()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            ServerAddress = GetString(appSettings, "ServerAddress", "localhost");
            ServerPort = GetInt(appSettings, "ServerPort", 9050);
            ServerConnectionKey = GetString(appSettings, "ServerConnectionKey");
        }
    }
}
