using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Account
{
    public interface IAccountService
    {
        Task<UserDetailsResponse> GetUserById();
        Task<ProfilePictureResponse> ChangeProfilePicture(ChangeProfilePictureRequest request);
        Task<GeneralResponse> UpdateUsername(string username);
        Task<GeneralResponse> DeleteAccount();
        Task<GeneralResponse> CreatePost(CreatePostRequest request);
        Task<UserProfileRespons> MyProfile();
        Task<UserProfileRespons> GetUserProfile(string userId);
        Task<GeneralResponse> ChangeActivities(List<ActivityCategory> activities);
        Task<GeneralResponse> GhostMode(bool ghostMode);
        Task<GeneralResponse> DoNotDisturb(bool doNotDisturb);
        Task<GeneralResponse> ReportUser(string userId);
        Task<GeneralResponse> UnbanUser(string userId);
        Task<GeneralResponse> BlockUser(string userId);
        Task<GeneralResponse> UnblockUser(string blockedUserId);
        //Task<BannedAccountDTO> GetBannedUser(string email);
        Task<UserBriefDetailsResponse> GetBlockedUsers(PageSettingsRequest model);
        Task<UserProfileRespons> GetBannedProfile(string userId);
        Task<BannedAccountResponse> GetBannedAccounts(PageSettingsRequest model);
        Task<GeneralResponse> Logout(LogoutRequest model);
    }
}
