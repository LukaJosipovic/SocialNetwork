using Application.DTO.Request;
using Application.DTO.Response;
using MobileClient.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Activity
{
    public class ActivityService : IActivityService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GeneralResponse> AcceptActivity(string cacheKey)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");

                var response = await client.PostAsJsonAsync($"api/Activity/AcceptActivity", cacheKey);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GeneralResponse> CreateActivity(CreateActivityRequest request)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.PostAsJsonAsync($"api/Activity/CreateActivity", request);
            var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return responseObject;
        }

        public async Task<List<ActivityResponse>> GetActivities()
        {
            var location = await LocationHelper.GetCurrentLocation();

            if (location == null)
            {
                location = new Microsoft.Maui.Devices.Sensors.Location
                {
                    Latitude = 45.83111,
                    Longitude = 16.11639
                };
            }

            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.GetAsync($"api/Activity/GetActivities?latitude={location.Latitude}&longitude={location.Longitude}");
            var responseObject = await response.Content.ReadFromJsonAsync<List<ActivityResponse>>();
            return responseObject;
        }

        public Task<GeneralResponse> ReportActivity(string cacheKey)
        {
            throw new NotImplementedException();
        }
    }
}
