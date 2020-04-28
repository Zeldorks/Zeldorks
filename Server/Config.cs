using System;
using System.Configuration;
using System.Collections.Specialized;

using static NetGameShared.Util.Config;

namespace NetGameServer
{
    public class Config
    {
        public int Port { get; set; }
        public string ConnectionKey { get; set; }
        public TimeSpan ArrowFireDelay { get; set; }
        public int ArrowSpeed { get; set; }
        public int BombSpeed { get; set; }
        public int FireballSpeed { get; set; }
        public int BoomerangSpeed { get; set; }
        public int KeySpeed { get; set; }
        public int SwordSlashSpeed { get; set; }
        public TimeSpan BoomerangFireDelay { get; set; }
        public int ScoreLimit { get; set; }

        public Config()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            Port = GetInt(appSettings, "Port", 9050);
            ConnectionKey = GetString(appSettings, "ConnectionKey");
            ArrowFireDelay = GetTimeSpan(
                appSettings,
                "ArrowFireDelay",
                TimeSpan.FromMilliseconds(200)
            );
            BoomerangFireDelay = GetTimeSpan(
                appSettings,
                "BoomerangFireDelay",
                TimeSpan.FromMilliseconds(2000)
            );

            ArrowSpeed = GetInt(appSettings, "ArrowSpeed", 20);
            BombSpeed = GetInt(appSettings, "BombSpeed", 0);
            KeySpeed = GetInt(appSettings, "KeySpeed", 0);
            SwordSlashSpeed = GetInt(appSettings, "SwordSlashSpeed", 0);
            BoomerangSpeed = GetInt(appSettings, "BoomerangSpeed", 10);
            ScoreLimit = GetInt(appSettings, "ScoreLimit", 10);
            FireballSpeed = GetInt(appSettings, "FireballSpeed", 8);
        }
    }
}
