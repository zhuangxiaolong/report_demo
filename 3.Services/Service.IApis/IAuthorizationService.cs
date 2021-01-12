using Domain.Core.Authorization;

namespace Service.IApis
{
    public interface IAuthorizationService
    {
         UserInfoRedis GetUserInfo(string ticket);
    }
}