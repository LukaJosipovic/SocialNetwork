using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using Application.Helper;
using Microsoft.AspNetCore.WebUtilities;
using MobileClient.BackgroundTask.Location;
using MobileClient.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<GeneralResponse> CreateActivity(CreateActivityRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsJsonAsync($"api/Activity/CreateActivity", request);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }
        public async Task<MyActivityResponse> MyActivities()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Activity/MyActivities");
                var responseObject = await response.Content.ReadFromJsonAsync<MyActivityResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateMyActivityResponse(false, ex.Message, null);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreateMyActivityResponse(false, "Something went wrong", null);
            }
        }

        public async Task<ActivityResponse> GetMyAcceptedActivities(PageSettingsRequest model)
        {
            try
            {   
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Activity/GetMyAcceptedActivities?PageNumber={model.PageNumber}&PageSize={model.PageSize}");
                var responseObject = await response.Content.ReadFromJsonAsync<ActivityResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateActivityResponse(false, ex.Message, null);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreateActivityResponse(false, "Something went wrong", null);
            }
        }

        public async Task<ActivityResponse> GetActivities()
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

            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var query = new Dictionary<string, string>
                {
                    ["latitude"] = location.Latitude.ToString(CultureInfo.InvariantCulture),
                    ["longitude"] = location.Longitude.ToString(CultureInfo.InvariantCulture)
                };

                var url = QueryHelpers.AddQueryString("api/Activity/GetActivities", query);
                var response = await client.GetAsync(url);
                var responseObject = await response.Content.ReadFromJsonAsync<ActivityResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateActivityResponse(false, ex.Message, null);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreateActivityResponse(false, "Something went wrong", null);
            }
        }
    }
}
