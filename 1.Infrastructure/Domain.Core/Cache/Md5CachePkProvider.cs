using Domain.Core.Common;

namespace Domain.Core.Cache
{
    public class Md5CachePkProvider : ICachePkProvider
    {
        public string GenerateKey(object pk)
        {
            return HashHelper.GetMd5(pk.ToString());
        }
    }
}