using Application.DTO.Request;
using Application.DTO.Response;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAuthRepository
    {
        Task<IdentityResult> CreateAccount(ApplicationUser user, string password, string role);
        Task <ApplicationUser> Login(LoginUserRequest model);
        Task <string> GenerateResetPasswordToken(ApplicationUser user);
        Task<IdentityResult> ResetPassword(ApplicationUser user, ResetPasswordRequest request);
        Task<IdentityResult> ConfirmEmail(ApplicationUser user, string token);
        Task<bool> GetUserByEmail(string email);
    }
}
