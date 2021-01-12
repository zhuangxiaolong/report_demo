using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core.APIClient;
using Microsoft.AspNetCore.Mvc;
using UI.Dtos.User;
using UI.IAssemblers;

namespace UI.Apis.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserAssembler _assembler;
        public UserController(IUserAssembler assembler)
        {
            _assembler = assembler;
        }
        [HttpPost]
        [Route("login")]
        public  ResultMessage<bool> login([FromBody]LoginRequestDto request)
        {
            return _assembler.login(request);
        }
        [HttpGet]
        [Route("check_login")]
        public  ResultMessage<bool> check_login()
        {
            return _assembler.check_login();
        }

    }
}
