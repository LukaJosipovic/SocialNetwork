using Application.DTO.Request;
using Application.DTO.Response;
using Application.Enum;
using System.Net.Http.Json;

namespace MobileClient.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ProfilePictureResponse> ChangeProfilePicture(ChangeProfilePictureRequest request)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.PutAsJsonAsync("api/Account/ChangeProfilePicture", request);
            var responseObject = await response.Content.ReadFromJsonAsync<ProfilePictureResponse>();
            return responseObject;
        }

        public async Task<GeneralResponse> CreatePost(CreatePostRequest request)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.PostAsJsonAsync($"api/Account/CreatePost", request);
            var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return responseObject;
        }

        public async Task<UserDetailsResponse> GetUserById()
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.GetAsync($"api/Account/GetUserDetails");
            var responseObject = await response.Content.ReadFromJsonAsync<UserDetailsResponse>();
            return responseObject;
        }

        public async Task<GeneralResponse> UpdateUsername(string username)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.PutAsync($"api/Account/UpdateUsername?username={username}", null);
            var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return responseObject;
        }
        
        public async Task<GeneralResponse> DeleteAccount()
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.PutAsync($"api/Account/DeleteAccount", null);
            var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return responseObject;
        }

        public async Task<UserProfileRespons> MyProfile()
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.GetAsync("api/Account/MyProfile");
            var responseObject = await response.Content.ReadFromJsonAsync<UserProfileRespons>();
            return responseObject;
        }

        public async Task<UserProfileRespons> GetUserProfile(string userId)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.GetAsync($"api/Account/GetUserProfile?userId={userId}");
            var responseObject = await response.Content.ReadFromJsonAsync<UserProfileRespons>();
            return responseObject;
        }

        public async Task<GeneralResponse> ChangeActivities(List<ActivityCategory> activities)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.PutAsJsonAsync($"api/Account/ChangeActivities", activities);
            var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return responseObject;
        }
        public async Task<GeneralResponse> GhostMode(bool ghostMode)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.PutAsync($"api/Account/GhostMode?ghostMode={ghostMode}", null);
            var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return responseObject;
        }
        
        public async Task<GeneralResponse> DoNotDisturb(bool doNotDisturb)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.PutAsync($"api/Account/DoNotDisturb?doNotDisturb={doNotDisturb}", null);
            var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return responseObject;
        }

        public async Task<GeneralResponse> ReportUser(string userId)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.PostAsync($"api/Account/ReportUser?userId={userId}", null);
            var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return responseObject;
        }
    }
}
