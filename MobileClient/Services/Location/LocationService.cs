using Application.DTO;
using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Helper;

namespace MobileClient.Services.Location
{
    public class LocationService : ILocationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LocationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<LocationDTO>> GetUsersLocations()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Location/GetLocations");
                var content = await response.Content.ReadAsStringAsync();
                var responseObject = await response.Content.ReadFromJsonAsync<List<LocationDTO>>();
                return responseObject;
            }
            catch (AccountBannedException)
            {

                throw;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> AddLocation(double latitude, double longitude)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsync($"api/Location/AddLocation?latitude={latitude}&longitude={longitude}", new StringContent(string.Empty));
                var responseObject = await response.Content.ReadFromJsonAsync<bool>();
                return responseObject;
            }
            catch (AccountBannedException)
            {

                throw;
            }
            catch (Exception ex)
            {
                //return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
                throw;
            }
        }
        
        public async Task<bool> RemoveLocation()
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.DeleteAsync($"api/Location/RemoveLocation");
            var responseObject = await response.Content.ReadFromJsonAsync<bool>();
            return responseObject;
        }
    }
}
