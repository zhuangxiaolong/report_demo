namespace Domain.Core
{
    public interface IConfigurationProvider : IDependency
    {
        string GetAppSetting(string key);
        string GetConnectionString(string key);
    }
}