using Application.DTO.Request;
using Application.DTO.Response;
using MobileClient.BackgroundTask.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using MobileClient.AuthStateProvider;
using Microsoft.AspNetCore.Components.Authorization;
using MobileClient.Services.Location;

namespace MobileClient.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundLocationService _backgroundLocationService;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(IHttpClientFactory httpClientFactory, IBackgroundLocationService backgroundLocationService, AuthenticationStateProvider authStateProvider)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundLocationService = backgroundLocationService;
            _authStateProvider = authStateProvider;
        }

        public async Task<LoginResponse> Login(LoginUserRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsJsonAsync("api/Auth/Login", request);
                var responseObject = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (responseObject.IsSuccess)
                {
                    var authProvider = (CustomAuthStateProvider)_authStateProvider;
                    authProvider.NotifyUserAuthentication(responseObject.AccessToken);
                    await _backgroundLocationService.Start();       
                }
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Something went wrong"
                };
            }
        }

        public async Task<RegisterResponse> Register(CreateAccountRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsJsonAsync("api/Auth/CreateAccount", request);
                if (response.IsSuccessStatusCode)
                {
                    return new RegisterResponse
                    {
                        IsSuccess = true,
                        Message = "Registration successful"
                    };
                }
                var responseObject = await response.Content.ReadFromJsonAsync<RegisterResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return new RegisterResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
            catch(Exception ex)
            {
                return new RegisterResponse
                {
                    IsSuccess = false,
                    Message = "Something went wrong"
                };
            }
        }

        public async Task<RegisterResponse> AddAdmin(CreateAccountRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsJsonAsync("api/Auth/AddAdmin", request);
                if (response.IsSuccessStatusCode)
                {
                    return new RegisterResponse
                    {
                        IsSuccess = true,
                        Message = "Registration successful"
                    };
                }
                var responseObject = await response.Content.ReadFromJsonAsync<RegisterResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                return new RegisterResponse
                {
                    IsSuccess = false,
                    Message = "Something went wrong"
                };
            }
        }

        public async Task<GeneralResponse> ForgotPassword(ForgotUserPasswordRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsJsonAsync("api/Auth/ForgotPassword", request);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
