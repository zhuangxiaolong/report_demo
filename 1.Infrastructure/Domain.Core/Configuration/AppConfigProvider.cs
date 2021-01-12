namespace Domain.Core.Configuration
{
    public class AppConfigProvider : IConfigurationProvider
    {
        public string GetAppSetting(string key)
        {
            //var appSetting = ConfigurationManager.AppSettings;
            //if (appSetting.AllKeys.Contains(key))
            //    return appSetting[key];
            return default(string);
        }

        public string GetConnectionString(string key)
        {
            //var connStrs = ConfigurationManager.ConnectionStrings;
            //return connStrs[key].ConnectionString ?? string.Empty;
            return default(string);
        }
    }
}