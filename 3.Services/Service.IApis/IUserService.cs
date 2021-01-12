using Domain.Core.APIClient;
using System;
using UI.Dtos.User;

namespace Service.IApis
{
    public interface IUserService
    {
        ResultMessage<bool> login(LoginRequestDto request);
    }
}
