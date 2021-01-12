using System;
using Domain.Core.APIClient;
using UI.Dtos.User;

namespace UI.IAssemblers
{
    public interface IUserAssembler
    {
        ResultMessage<bool> login(LoginRequestDto request);
        ResultMessage<bool> check_login();
    }
}
