namespace Domain.Core
{
    public interface IConfiguration : IInitializable
    {
        IConfigurationProvider Provider { get; }
    }
}