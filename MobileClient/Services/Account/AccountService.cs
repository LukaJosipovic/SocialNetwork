using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Enum;
using Application.Exceptions;
using Application.Helper;
using Domain.Model;
using MobileClient.BackgroundTask.Location;
using MobileClient.Services.Location;
using System.Net.Http.Json;

namespace MobileClient.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundLocationService _backgroundLocationService;

        public AccountService(IHttpClientFactory httpClientFactory, IBackgroundLocationService backgroundLocationService)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundLocationService = backgroundLocationService;
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
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<GeneralResponse> UpdateDescription(string description)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PutAsync($"api/Account/UpdateDescription?description={description}", null);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<GeneralResponse> UpdateUserDetails(UpdateUserDetailsRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsJsonAsync($"api/Account/UpdateUserDetails", request);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
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
            catch (Exception ex)
            {
                return ResponseHelper.CreateUserProfileRespons(null, null, null, false, "Something went wrong");
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
            catch (Exception ex)
            {
                return ResponseHelper.CreateUserProfileRespons(null, null, null, false, "Something went wrong");
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
            catch (Exception ex)
            {
                return ResponseHelper.CreateUserProfileRespons(null, null, null, false, "Something went wrong");
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
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        //public async Task<BannedAccountResponse> GetBannedUser(string email)
        //{
        //    try
        //    {
        //        var client = _httpClientFactory.CreateClient("BaseApi");
        //        var response = await client.GetAsync($"api/Account/GetBannedUser?email={email}");
        //        var responseObject = await response.Content.ReadFromJsonAsync<BannedAccountResponse>();
        //        return responseObject;
        //    }
        //    catch (AccountBannedException ex)
        //    {
        //        return ResponseHelper.CreateBannedUserResponse(false, ex.Message, null);
        //    }
        //    catch (Exception)
        //    {
        //        return ResponseHelper.CreateBannedUserResponse(false, "Something went wrong", null);
        //    }
        //}

        public async Task<GeneralResponse> UnbanUser(string userId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsync($"api/Account/UnbanUser?userId={userId}", null);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
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
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<UserBriefDetailsResponse> GetBlockedUsers(PageSettingsRequest model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Account/GetBlockedUsers?PageNumber={model.PageNumber}&PageSize={model.PageSize}");
                var responseObject = await response.Content.ReadFromJsonAsync<UserBriefDetailsResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateUserBriefDetailsResponse(false, "Something went wrong", null);
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
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<BannedAccountResponse> GetBannedAccounts(PageSettingsRequest model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Account/GetBannedAccounts?PageNumber={model.PageNumber}&PageSize={model.PageSize}&SearchTerm={model.SearchTerm}");
                var responseObject = await response.Content.ReadFromJsonAsync<BannedAccountResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateBannedAccountsResponse(false, "Something went wrong", null);
            }
        }

        public async Task<GeneralResponse> Logout(LogoutRequest model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PutAsJsonAsync($"api/Account/Logout", model);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                await SecureStorage.SetAsync("ShouldRunLocationService", "false");
                await _backgroundLocationService.Stop();
                return responseObject;
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<GeneralResponse> SendFriendRequest(string userId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsync($"api/Account/SendFriendRequest?receiverUserId={userId}", null);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }
        public async Task<FriendRequestResponse> GetFriendRequests()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Account/GetFriendRequest");
                var responseObject = await response.Content.ReadFromJsonAsync<FriendRequestResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateFriendRequestResponse(false, "Something went wrong", null);
            }
        }

        public async Task<GeneralResponse> AcceptFriendship(string userId, int friendRequestId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsync($"api/Account/AcceptFriendship?senderId={userId}&friendRequestId={friendRequestId}", null);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<UserBriefDetailsResponse> GetMatches(PageSettingsRequest model, string? UserId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Account/GetMatches?PageNumber={model.PageNumber}&PageSize={model.PageSize}&UserId={UserId}");
                var responseObject = await response.Content.ReadFromJsonAsync<UserBriefDetailsResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateUserBriefDetailsResponse(false, "Something went wrong", null);
            }
        }

        public async Task<UserBriefDetailsResponse> GetFriends(PageSettingsRequest model, string? UserId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Account/GetFriends?PageNumber={model.PageNumber}&PageSize={model.PageSize}&UserId={UserId}");
                var responseObject = await response.Content.ReadFromJsonAsync<UserBriefDetailsResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateUserBriefDetailsResponse(false, "Something went wrong", null);
            }
        }
    }
}
