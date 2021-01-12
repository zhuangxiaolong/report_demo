using System;
using Domain.Core.APIClient;
using Service.IApis;
using UI.Core;
using UI.Dtos.User;
using UI.IAssemblers;

namespace UI.Assemblers
{
    public class UserAssembler:AssemblerBase,IUserAssembler
    {
        private IUserService _service;

        public UserAssembler(IUserService service)
        {
            _service = service;
        }
        public ResultMessage<bool> login(LoginRequestDto request)
        {
            var response = new ResultMessage<bool>();

            var result = _service.login(request);

            response.body = result.body;
            response.err_code = result.err_code;
            response.err_msg = result.err_msg;

            return response;
        }
        public ResultMessage<bool> check_login()
        {
            var response = new ResultMessage<bool>();

            var userInfo = GetUserData();
            if (userInfo == null)
            {
                response.body = false;
                response.err_code =400;
                response.err_msg = "无登录信息";
                return response;
            }
            
            response.body = true;
            response.err_code =200;
            return response;
        }
    }
}
