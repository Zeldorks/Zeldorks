using System;
using System.Collections.Specialized;

namespace NetGameShared.Util
{
    public static class Config
    {
        public static string GetString(
            NameValueCollection appSettings,
            string key,
            string defaultValue = ""
        ) {
            return appSettings[key] ?? defaultValue;
        }

        public static TimeSpan GetTimeSpan(
            NameValueCollection appSettings,
            string key,
            TimeSpan defaultValue
        ) {
            string stringValue = appSettings[key];

            if (stringValue == null) {
                return defaultValue;
            } else {
                TimeSpan parsedValue;
                if (TimeSpan.TryParse(stringValue, out parsedValue)) {
                    return parsedValue;
                } else {
                    return defaultValue;
                }
            }
        }

        public static int GetInt(
            NameValueCollection appSettings,
            string key,
            int defaultValue
        ) {
            string stringValue = appSettings[key];

            if (stringValue == null) {
                return defaultValue;
            } else {
                int parsedValue;
                if (int.TryParse(stringValue, out parsedValue)) {
                    return parsedValue;
                } else {
                    return defaultValue;
                }
            }
        }
    }
}
