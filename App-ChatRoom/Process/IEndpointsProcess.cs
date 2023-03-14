using App_Chat_Common.Responses;
using App_Chat_Common.ViewModels;

namespace App_ChatRoom.Process
{
    public interface IEndpointsProcess
    {
        Task<LoginResponse> Login(LoginViewModel request);
    }
}