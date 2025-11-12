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

namespace MobileClient.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("BaseApi");

        private async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, TResponse fallback)
        {
            try
            {
                var client = CreateClient();
                var response = await client.PostAsJsonAsync(url, request);
                
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<TResponse>();
                

                var errorResponse = await response.Content.ReadFromJsonAsync<TResponse>();
                return errorResponse ?? fallback;
            }
            catch
            {
                return fallback;
            }
        }

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
            //try
            //{
            //    var client = _httpClientFactory.CreateClient("BaseApi");
            //    var resultJwtTest = await client.GetAsync("api/Auth/JwtTest");
            //    var response = await client.PostAsJsonAsync("api/Auth/Login", request);
            //    var responseObject = await response.Content.ReadFromJsonAsync<LoginResponse>();
            //    return responseObject;
            //}
            //catch (Exception ex)
            //{
            //    return new LoginResponse
            //    {
            //        IsSuccess = false,
            //        Message = "Something went wrong please try again later"
            //    };
            //}
            var response = await PostAsync("api/Auth/Login", request, new LoginResponse
            {
                IsSuccess = false,
                Message = "Something went wrong please try again later"
            });
            
            return response;
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
            catch (Exception ex)
            {
                return new RegisterResponse
                {
                    IsSuccess = false,
                    Message = "Something went wrong please try again later"
                };
            }
            //return await PostAsync("api/Auth/CreateAccount", request, new RegisterResponse
            //{
            //    IsSuccess = false,
            //    Message = "Something went wrong please try again later"
            //});
        }

        public async Task<RegisterResponse> AddAdmin(CreateAccountRequest request)
        {
            //try
            //{
            //    var client = _httpClientFactory.CreateClient("BaseApi");
            //    var response = await client.PostAsJsonAsync("api/Auth/AddAdmin", request);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        return new RegisterResponse
            //        {
            //            IsSuccess = true,
            //            Message = "Registration successful"
            //        };
            //    }
            //    var responseObject = await response.Content.ReadFromJsonAsync<RegisterResponse>();
            //    return responseObject;
            //}
            //catch (Exception ex)
            //{
            //    return new RegisterResponse
            //    {
            //        IsSuccess = false,
            //        Message = "Something went wrong please try again later"
            //    };
            //}
            return await PostAsync("api/Auth/AddAdmin", request, new RegisterResponse
            {
                IsSuccess = false,
                Message = "Something went wrong please try again later"
            });
        }

        public async Task<GeneralResponse> ForgotPassword(ForgotUserPasswordRequest request)
        {
            //try
            //{
            //    var client = _httpClientFactory.CreateClient("BaseApi");
            //    var response = await client.PostAsJsonAsync("api/Auth/ForgotPassword", request);
            //    var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            //    return responseObject;
            //}
            //catch (Exception ex)
            //{
            //    return new GeneralResponse
            //    {
            //        IsSuccess = false,
            //        Message = "Something went wrong"
            //    };
            //}
            return await PostAsync("api/Auth/ForgotPassword", request, new GeneralResponse
            {
                IsSuccess = false,
                Message = "Something went wrong"
            });
        }
    }
}
