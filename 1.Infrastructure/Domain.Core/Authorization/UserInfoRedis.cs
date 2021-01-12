using Domain.Core.Cache;

namespace Domain.Core.Authorization
{
    public class UserInfoRedis:  CacheableObjectBase
    {
        public string name { get; set; }
        public string date_time { get; set; }
    }
}