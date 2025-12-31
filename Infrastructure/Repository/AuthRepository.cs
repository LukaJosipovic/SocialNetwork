using Application.Contracts;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly AppDbContext _appDbContext;

        public AuthRepository(UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtTokenGenerator, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _appDbContext = appDbContext;
        }

        public async Task<IdentityResult> ConfirmEmail(ApplicationUser user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<IdentityResult> CreateAccount(ApplicationUser user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);
            
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, role);
            
            return result;
        }

        public async Task<bool> GetUserByEmail(string email)
        {
            var result = await _userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Email == email);

            if (result == null)
                return false;

            return true;
        }

        public async Task<string> GenerateResetPasswordToken(ApplicationUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<ApplicationUser> Login(LoginUserRequest model)
        {
            var user = await _userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Email == model.Email) ?? throw new UnauthorizedAccessException("Invalid email or password");

            if (user.IsBanned)
                throw new AccountBannedException();

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                throw new UnauthorizedAccessException("Invalid email or password");

            return user;
        }

        public async Task<IdentityResult> ResetPassword(ApplicationUser user, ResetPasswordRequest request)
        {
            return await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        }
    }
}
