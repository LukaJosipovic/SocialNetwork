using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Enum;
using Application.Exceptions;
using Application.Helper;
using MobileClient.BackgroundTask.Location;
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
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PutAsJsonAsync("api/Account/ChangeProfilePicture", request);
                var responseObject = await response.Content.ReadFromJsonAsync<ProfilePictureResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateProfilePictureResponse(false, ex.Message, null);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateProfilePictureResponse(false, "Something went wrong", null);
            }
        }

        public async Task<GeneralResponse> CreatePost(CreatePostRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsJsonAsync($"api/Account/CreatePost", request);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<UserDetailsResponse> GetUserById()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Account/GetUserDetails");
                var responseObject = await response.Content.ReadFromJsonAsync<UserDetailsResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateUserDetailsResponse(null, false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateUserDetailsResponse(null, false, "Something went wrong");
            }
        }

        public async Task<GeneralResponse> UpdateUsername(string username)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PutAsync($"api/Account/UpdateUsername?username={username}", null);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }
        
        public async Task<GeneralResponse> DeleteAccount()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PutAsync($"api/Account/DeleteAccount", null);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<UserProfileRespons> MyProfile()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync("api/Account/MyProfile");
                var responseObject = await response.Content.ReadFromJsonAsync<UserProfileRespons>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateUserProfileRespons(null, false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateUserProfileRespons(null, false, "Something went wrong");
            }
        }

        public async Task<UserProfileRespons> GetUserProfile(string userId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Account/GetUserProfile?userId={userId}");
                var responseObject = await response.Content.ReadFromJsonAsync<UserProfileRespons>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateUserProfileRespons(null, false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateUserProfileRespons(null, false, "Something went wrong");
            }
        }

        public async Task<UserProfileRespons> GetBannedProfile(string userId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Account/GetBannedProfile?userId={userId}");
                var responseObject = await response.Content.ReadFromJsonAsync<UserProfileRespons>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateUserProfileRespons(null, false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateUserProfileRespons(null, false, "Something went wrong");
            }
        }

        public async Task<GeneralResponse> ChangeActivities(List<ActivityCategory> activities)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PutAsJsonAsync($"api/Account/ChangeActivities", activities);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }
        public async Task<GeneralResponse> GhostMode(bool ghostMode)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PutAsync($"api/Account/GhostMode?ghostMode={ghostMode}", null);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }
        
        public async Task<GeneralResponse> DoNotDisturb(bool doNotDisturb)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PutAsync($"api/Account/DoNotDisturb?doNotDisturb={doNotDisturb}", null);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<GeneralResponse> ReportUser(string userId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsync($"api/Account/ReportUser?userId={userId}", null);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<BannedAccountResponse> GetBannedUser(string email)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Account/GetBannedUser?email={email}");
                var responseObject = await response.Content.ReadFromJsonAsync<BannedAccountResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateBannedUserResponse(false, ex.Message, null);
            }
            catch (Exception)
            {
                return ResponseHelper.CreateBannedUserResponse(false, "Something went wrong", null);
            }
        }

        public async Task<GeneralResponse> UnbanUser(string userId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsync($"api/Account/UnbanUser?userId={userId}", null);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<GeneralResponse> BlockUser(string userId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsync($"api/Account/BlockUser?userIdToBlock={userId}", null);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<List<UserBriefDetailsDTO>> GetBlockedUsers(PageSettingsRequest model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Account/GetBlockedUsers?PageNumber={model.PageNumber}&PageSize={model.PageSize}");
                var responseObject = await response.Content.ReadFromJsonAsync<List<UserBriefDetailsDTO>>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {

                throw;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GeneralResponse> UnblockUser(string blockedUserId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.DeleteAsync($"api/Account/UnblockUser?blockedUserId={blockedUserId}");
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (AccountBannedException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }
    }
}
