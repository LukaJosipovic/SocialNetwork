using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using MobileClient.SessionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Handlers
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserSessionService _userSessionService;

        public TokenHandler(IHttpClientFactory httpClientFactory, UserSessionService userSessionService)
        {
            _httpClientFactory = httpClientFactory;
            _userSessionService = userSessionService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await SecureStorage.GetAsync("accessToken");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshSuccess = await TryRefreshTokenAsync(token, cancellationToken);
                if (refreshSuccess)
                {
                    response.Dispose(); // Dispose the old response

                    //var newRequest = await CloneHttpRequestMessageAsync(request);
                    
                    // Retry the original request with the new access token
                    token = await SecureStorage.GetAsync("accessToken");

                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    //newRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    //response.Dispose(); 

                    // Retry the same request
                    response = await base.SendAsync(request, cancellationToken);
                }
            }
            return response;
        }
        private async Task<bool> TryRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            var refreshToken = await SecureStorage.GetAsync("refreshToken");

            if (string.IsNullOrEmpty(refreshToken))
                return false;

            var refreshRequest = new RefreshTokenRequest
            {
                JwtToken = token,
                RefreshToken = refreshToken
            };

            var client = _httpClientFactory.CreateClient("BaseApi");

            var response = await client.PostAsJsonAsync("/api/auth/Refresh", refreshRequest, cancellationToken);

            //if (!response.IsSuccessStatusCode)
            //    return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result.IsBanned == true)
            {
                await _userSessionService.TriggerBannedAsync();
                throw new AccountBannedException();
                //return false;
            }

            if (result == null)
                return false;

            //if (result.IsBanned)
            //{
            //    _userSessionService.TriggerBanned("Your account has been banned");
            //    return false;
            //}

            await SecureStorage.SetAsync("accessToken", result.AccessToken);
            await SecureStorage.SetAsync("refreshToken", result.RefreshToken);

            return true;
        }

        private static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            // Copy headers
            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            // Copy content
            if (request.Content != null)
            {
                var ms = new MemoryStream();
                await request.Content.CopyToAsync(ms);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                foreach (var header in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }
    }
}
