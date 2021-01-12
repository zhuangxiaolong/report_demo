using Domain.Core.Authorization;
using Domain.Core.Cache;
using Domain.Data;
using Domain.Models;
using Service.IApis;
using Services.Core.Services;

namespace Service.Apis
{
    public class AuthorizationService: ServiceBase,IAuthorizationService
    {
        private ICacheClient _cacheClient;

        public AuthorizationService(
            ICacheClient cacheClient
        )
            : base("AuthorizationService:")
        {
            _cacheClient = cacheClient;
        }

        public UserInfoRedis GetUserInfo(string ticket)
        {
            var obj=_cacheClient.Get<UserInfoRedis>(ticket);
            return obj;
        }
    }
}