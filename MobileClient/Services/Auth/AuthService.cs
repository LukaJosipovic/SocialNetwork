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

#if ANDROID
using MobileClient.Platforms.Android.BackgroundService;
#endif

namespace MobileClient.Services.Auth
{
    public class AuthService : IAuthService
    {
#if ANDROID
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundLocationService _backgroundLocationService;

        public AuthService(IHttpClientFactory httpClientFactory, IBackgroundLocationService backgroundLocationService)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundLocationService = backgroundLocationService;
        }
#else
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocationTracker _locationTracker;

        public AuthService(IHttpClientFactory httpClientFactory, ILocationTracker locationTracker)
        {
            _httpClientFactory = httpClientFactory;
            _locationTracker = locationTracker;
        }
#endif
        private HttpClient CreateClient() => _httpClientFactory.CreateClient("BaseApi");

        //private async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, TResponse fallback)
        //{
        //    try
        //    {
        //        var client = CreateClient();
        //        var response = await client.PostAsJsonAsync(url, request);
                
        //        if (response.IsSuccessStatusCode)
        //            return await response.Content.ReadFromJsonAsync<TResponse>();
                

        //        var errorResponse = await response.Content.ReadFromJsonAsync<TResponse>();
        //        return errorResponse ?? fallback;
        //    }
        //    catch
        //    {
        //        return fallback;
        //    }
        //}

        public async Task<string> Test()
        {
            var token = await SecureStorage.GetAsync("accessToken");
            var client = _httpClientFactory.CreateClient("BaseApi");
            var resultJwtTest = await client.GetAsync("api/Auth/JwtTest");
            var responseObject = await resultJwtTest.Content.ReadAsStringAsync();
            return responseObject;
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
#if ANDROID
                    await _backgroundLocationService.Start();
#else
                    await _locationTracker.StartAsync();
#endif             
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
