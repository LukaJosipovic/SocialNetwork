using Application.DTO.Request;
using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Auth
{
    public interface IAuthService
    {
        Task<RegisterResponse> Register(CreateAccountRequest request);
        Task<RegisterResponse> AddAdmin(CreateAccountRequest request);
        Task<LoginResponse> Login(LoginUserRequest request);
        Task<GeneralResponse> ForgotPassword(ForgotUserPasswordRequest request);
        Task<string> Test();
    }
}
