using Domain.Core.APIClient;
using System;
using System.Linq;
using Domain.Core.Authorization;
using Domain.Core.Cache;
using Domain.Core.Common;
using Domain.Core.Log;
using Domain.Data;
using Domain.Models;
using Service.IApis;
using UI.Dtos.User;
using Services.Core.Services;
using Domain.Repository.EF;

namespace Service.Apis
{
    public class UserService: ServiceBase,IUserService
    {
        private IRepository<UserInfo, int> _userRep;
        private ICacheClient _cacheClient;
        public UserService(IRepository<UserInfo, int> userRep
        ,ICacheClient cacheClient
        )
            : base("UserService:")
        {
            _userRep = userRep;
            _cacheClient = cacheClient;
        }
        public ResultMessage<bool> login(LoginRequestDto request)
        {
            var response = new ResultMessage<bool>()
            {
                err_code = 400,
                err_msg = "用户名或密码错误",
            };

            return Logger("登录接口", () =>
            {
                var obj = _userRep.Find(r => r.Name == request.user_name
                                             && r.Pw == request.pw).FirstOrDefault();
                if (obj != null)
                {
                    #region 加入redis

                    var cacheId = Guid.NewGuid().ToString().Replace("-", "");
                    var token = HashHelper.GetMd5ForUTF8(cacheId.ToString());

                    var userInfo = new UserInfoRedis
                    {
                        CacheId = token,
                        name = obj.Name,
                        date_time = DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    var expiresDate = DateTime.Now.Date.AddDays(1);
                    var expiresTime = expiresDate - DateTime.Now;
                    _cacheClient.Add(userInfo);

                    #endregion
                    response.body = true;
                    response.err_code = 200;
                    response.err_msg = "登录成功";
                }
                return response;
            }, ErrorHandles.Continue, LogModes.Error, ex =>
            {
                return response;
            });

        }
    }
}
