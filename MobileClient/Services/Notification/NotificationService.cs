using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NotificationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GeneralResponse> RegisterDevice(string deviceToken)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.PostAsync($"api/Notification/RegisterDevice?deviceToken={deviceToken}", null);
            var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return responseObject;
        }
    }
}
