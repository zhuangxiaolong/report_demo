namespace Domain.Core.Cache
{
    public interface ICachePkProvider : IDependency
    {
        string GenerateKey(object pk);
    }
}