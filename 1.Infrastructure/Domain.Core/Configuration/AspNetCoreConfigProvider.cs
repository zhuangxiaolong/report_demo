using Microsoft.Extensions.Configuration;

namespace Domain.Core.Configuration
{
    public class AspNetCoreConfigProvider : IConfigurationProvider
    {
        private Microsoft.Extensions.Configuration.IConfiguration _configuration;
        public AspNetCoreConfigProvider(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetAppSetting(string key)
        {
            var array = key.Split(':');
            if (array?.Length == 2)
            {
                var section = _configuration.GetSection(array[0]);
                return section[array[1]];
            }
            return _configuration[key];
        }

        public string GetConnectionString(string key)
        {
            return _configuration.GetConnectionString(key);
        }
    }
}