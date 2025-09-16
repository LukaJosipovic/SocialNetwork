using Application.DTO.Request;
using Application.DTO.Response;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Auth
{
    public interface IAuthService
    {
        Task<RegisterResponse> CreateAccount(CreateAccountRequest model, string role);
        Task<LoginResponse> Login(LoginUserRequest model);
        Task<LoginResponse> RefreshToken(RefreshTokenRequest request);
        Task<GeneralResponse> ForgotPassword(ForgotUserPasswordRequest request);
        Task<GeneralResponse> ResetPassword(ResetPasswordRequest request);
        Task<GeneralResponse> ConfirmEmail(string userId, string token);
    }
}
