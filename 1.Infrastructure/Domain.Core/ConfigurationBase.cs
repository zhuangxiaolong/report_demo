using System;

namespace Domain.Core
{
    public abstract class ConfigurationBase : IConfiguration
    {
        public ConfigurationBase(IConfigurationProvider provider)
        {
            Provider = provider;
        }

        public IConfigurationProvider Provider { get; }

        public abstract void Initialize();

        protected string getString(string key, string defaultVal = "")
        {
            return Provider.GetAppSetting(key) ?? defaultVal;
        }

        protected bool getBoolean(string key, bool defaultVal = false)
        {
            var value = Provider.GetAppSetting(key);
            if (string.IsNullOrWhiteSpace(value)) return defaultVal;
            value = value.ToLower();
            bool result = defaultVal;
            if (value == "yes" || value == "1" || value == "on") return true;
            if (value == "no" || value == "0" || value == "off") return false;
            if (bool.TryParse(value, out result)) return result;
            return defaultVal;
        }

        protected DateTime getDatetime(string key, DateTime defaultVal)
        {
            var value = Provider.GetAppSetting(key);
            var result = defaultVal;
            if (DateTime.TryParse(value, out result)) return result;
            return defaultVal;
        }

        protected int getInteger(string key,int defaultVal = 0)
        {
            var value = Provider.GetAppSetting(key);
            var result = defaultVal;
            if (int.TryParse(value, out result)) return result;
            return defaultVal;
        }

        protected long getLong(string key, long defaultVal = 0)
        {
            var value = Provider.GetAppSetting(key);
            var result = defaultVal;
            if (long.TryParse(value, out result)) return result;
            return defaultVal;
        }

        protected double getDouble(string key, double defaultVal = 0)
        {
            var value = Provider.GetAppSetting(key);
            var result = defaultVal;
            if (double.TryParse(value, out result)) return result;
            return defaultVal;
        }

        protected float getFloat(string key, float defaultVal = 0)
        {
            var value = Provider.GetAppSetting(key);
            var result = defaultVal;
            if (float.TryParse(value, out result)) return result;
            return defaultVal;
        }
    }
}