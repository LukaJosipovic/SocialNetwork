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
        private readonly SemaphoreSlim _refreshSemaphore = new SemaphoreSlim(1, 1);

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
                bool refreshSuccess = false;

                try
                {
                    await _refreshSemaphore.WaitAsync(cancellationToken);

                    // Double check - maybe another thread already refreshed
                    var currentToken = await SecureStorage.GetAsync("accessToken");
                    if (currentToken != token) // Token was already updated
                    {
                        refreshSuccess = true;
                    }
                    else
                    {

                        refreshSuccess = await TryRefreshTokenAsync(token, cancellationToken);
                    }
                }
                finally
                {

                    _refreshSemaphore.Release();
                }
                if (refreshSuccess)
                {
                    // Retry the original request with the new access token
                    token = await SecureStorage.GetAsync("accessToken");

                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

            var client = _httpClientFactory.CreateClient("RefreshClient");

            var response = await client.PostAsJsonAsync("/api/auth/Refresh", refreshRequest, cancellationToken);

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result == null || !result.IsSuccess)
                return false;

            if (result.IsBanned == true)
            {
                await _userSessionService.TriggerBannedAsync();
            }


            await SecureStorage.SetAsync("accessToken", result.AccessToken);
            await SecureStorage.SetAsync("refreshToken", result.RefreshToken);

            return true;
        }
    }
}
