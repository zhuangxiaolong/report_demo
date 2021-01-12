using Domain.Core.Authorization;
using Domain.Core.Http;
using Microsoft.AspNetCore.Http;
using Service.IApis;

namespace UI.Core
{
    public abstract class AssemblerBase : IAssembler
    {
        private readonly IAuthorizationService _authorizationService;
        protected AssemblerBase()
        {

        }
        public AssemblerBase(IAuthorizationService authorization)
        {
            _authorizationService = authorization;
        }
        
        protected HttpContext Current = HttpContextCurrent.HttpContext;
        protected UserInfoRedis GetUserData()
        {
            var header =Current.Request.Headers;
            foreach (string key in header.Keys)
            {
                if (key != "Authorization")
                    continue;
                var obj=_authorizationService.GetUserInfo(header[key]);
                return obj;
            }
            return null;
        }
    }
}