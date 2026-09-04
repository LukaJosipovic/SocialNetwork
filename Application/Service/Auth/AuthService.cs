using Application.Contracts;
using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using Application.Helper;
using Application.Service.Email;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using MimeKit.Encodings;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailService _emailService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(IJwtTokenGenerator jwtTokenGenerator, IConfiguration configuration, IAuthRepository authRepository, IAccountRepository accountRepository, IEmailService emailService, UserManager<ApplicationUser> userManager, IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _configuration = configuration;
            _authRepository = authRepository;
            _accountRepository = accountRepository;
            _emailService = emailService;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<RegisterResponse> CreateAccount(CreateAccountRequest model, string role)
        {
            try
            {
                var userExists = await _authRepository.GetUserByEmail(model.Email);

                if (userExists)
                    return ResponseHelper.CreateRegisterResponse(false, null, new List<string> { "Email already exists" });

                var basePath = Path.GetDirectoryName(Environment.CurrentDirectory);
                var filePath = Path.Combine(basePath, "Img", "unknown.png");
                var imageByte = await File.ReadAllBytesAsync(filePath);

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Name = model.Username,
                    ProfilePicture = imageByte,
                    EmailConfirmed = false,
                    GhostMode = true,
                    DoNotDisturb = true,
                };

                var result = await _authRepository.CreateAccount(user, model.Password, role);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedToken = Encoding.UTF8.GetBytes(token);
                    token = WebEncoders.Base64UrlEncode(encodedToken);

                    string ip = await GetPublicIpAddressHelper.GetPublicIpAddress();
                    var url = $"http://{ip}:4321/socialnetwork/api/Auth/ConfirmEmail?userid={user.Id}&token={token}";

                    var email = new EmailDTO
                    {
                        To = user.Email,
                        Subject = "Verification email",
                        Body = $"Verify your email by clicking <a href='{url}'>here</a> and then log in to the application"
                    };
                    _emailService.SendEmail(email);

                    return ResponseHelper.CreateRegisterResponse(true, "Registration is successful", null);
                }
                return ResponseHelper.CreateRegisterResponse(false, "Registration failed", result.Errors.Select(e => e.Description));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateRegisterResponse(false, "Something went wrong", null);
            }
        }

        public async Task<LoginResponse> Login(LoginUserRequest model)
        {
            try
            {
                var user = await _authRepository.Login(model);
                var token = await _jwtTokenGenerator.GenerateToken(user.Id, user.Name, user.Email);
                var refreshToken = _refreshTokenRepository.GenerateRefreshToken();
                await _refreshTokenRepository.StoreRefreshToken(user, refreshToken);
                return ResponseHelper.CreateLoginResponse(true, null, token, user.Id, refreshToken);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ResponseHelper.CreateLoginResponse(false, ex.Message, null, null, null);
            } 
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateLoginResponse(false, ex.Message, null, null, null);
            }
        }

        public async Task<LoginResponse> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                var refreshToken = await _refreshTokenRepository.GetByTokenasync(request.RefreshToken);
                var user = await _accountRepository.GetAnyUserById(request.UserId);

                if (refreshToken.Token != request.RefreshToken || refreshToken.UserId != request.UserId || refreshToken.IsRevoked)
                    return ResponseHelper.CreateLoginResponse(false, "Something went wrong", null, null, null);

                if (user.IsBanned)
                    throw new AccountBannedException();

                string token = await _jwtTokenGenerator.GenerateToken(user.Id, user.Name, user.Email);

                if (refreshToken.Expires < DateTime.Now)
                {
                    var newRefreshToken = _refreshTokenRepository.GenerateRefreshToken();
                    await _refreshTokenRepository.RotateTokenAsync(request.RefreshToken, newRefreshToken, user.Id);
                    return ResponseHelper.CreateLoginResponse(true, null, token, user.Id, newRefreshToken);
                }
                return ResponseHelper.CreateLoginResponse(true, null, token, user.Id, refreshToken.Token);
            }
            catch (AccountBannedException ex)
            {
                return new LoginResponse
                {
                    IsSuccess = true,
                    IsBanned = true,
                    Message = ex.Message
                };
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateLoginResponse(false, ex.Message, null, null, null);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateLoginResponse(false, "Something went wrong", null, null, null);
            }
        }

        public async Task<GeneralResponse> ForgotPassword(ForgotUserPasswordRequest request)
        {
            try
            {
                var user = await _accountRepository.GetUserByEmail(request.Email);
                var token = await _authRepository.GenerateResetPasswordToken(user);

                var encodedToken = Encoding.UTF8.GetBytes(token);
                token = WebEncoders.Base64UrlEncode(encodedToken);

                string ip = await GetPublicIpAddressHelper.GetPublicIpAddress();
                var url = $"http://{ip}:4321/socialnetwork/resetpassword?token={token}&email={user.Email}";

                var email = new EmailDTO
                {
                    To = user.Email,
                    Subject = "Reset Password",
                    Body = $"To reset your password click <a href='{url}'>here</a>"
                };
                _emailService.SendEmail(email);

                return ResponseHelper.CreateGeneralResponse(true, "A password reset email has been sent to you. Follow the steps and try to login again.");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);    
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
        }

        public async Task<GeneralResponse> ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                var user = await _accountRepository.GetUserByEmail(request.Email);
                var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
                request.Token = Encoding.UTF8.GetString(decodedToken);

                var result = await _authRepository.ResetPassword(user, request);

                if (result.Succeeded) 
                    return ResponseHelper.CreateGeneralResponse(true, "Password has been successfully changed");

                return ResponseHelper.CreateGeneralResponse(true, "Something went wrong");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
        }

        public async Task<GeneralResponse> ConfirmEmail(string userId, string token)
        {
            try
            {
                var user = await _accountRepository.GetUserById(userId);
                var decodedToken = WebEncoders.Base64UrlDecode(token);
                token = Encoding.UTF8.GetString(decodedToken);

                var result = await _authRepository.ConfirmEmail(user, token);
                
                if (result.Succeeded)
                    return ResponseHelper.CreateGeneralResponse(true, "Email confirmed successfully");

                return ResponseHelper.CreateGeneralResponse(true, "Something went wrong");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }
    }
}
