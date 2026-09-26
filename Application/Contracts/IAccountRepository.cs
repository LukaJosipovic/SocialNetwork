using Application.DTO.Request;
using Application.DTO.Response;
using Application.Enum;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAccountRepository
    {
        Task<ApplicationUser> GetUserById(string userId);
        Task<IdentityResult> ChangeProfilePicture(ChangeProfilePictureRequest request, string userId);
        Task<IdentityResult> UpdateUsername(string username, string userId);
        Task<IdentityResult> UpdateDescription(string description, string userId);
        Task<IdentityResult> UpdateUserDetails(UpdateUserDetailsRequest request, string userId);
        Task<IdentityResult> DeleteAccount(string userId, byte[] imageByte);
        Task<bool> CreatePost(Post request, byte[]? imageData);
        Task<ApplicationUser> GetUserProfile(string userId);
        Task<List<ApplicationUser>> GetUsers();
        Task<IdentityResult> ChangeActivities(List<string> activities, string userId);
        Task<IdentityResult> GhostMode(bool ghostMode, string userId);
        Task<IdentityResult> DoNotDisturb(bool doNotDisturb, string userId);
        Task<bool> ReportUser(Report report);
        Task<bool> CheckIfReportExist(string reporterId, int postId);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<ApplicationUser> GetBannedUser(string email);
        Task<bool> CheckIfUserIsReported(string reporterId, string userId);
        Task<IdentityResult> BanAccount(string userId);
        Task<int> GetReportCount(string userId);
        Task<IdentityResult> UnbanUser(string userId);
        Task<bool> BlockUser(UserBlocks block);
        Task<bool> UnblockUser(UserBlocks block);
        Task<UserBlocks> GetBlockedUser(string blockerId, string blockedUserId);
        Task<List<UserBlocks>> GetAllBlockedUsers(string userId, PageSettingsRequest model);
        Task<ApplicationUser> GetAnyUserById(string userId);
        Task<List<string>> GetDeviceTokensByIdRange(List<string> IdRange, string category);
        Task<List<string>> FilterUserIdRange(List<string> IdRange, string category, string userId);
        Task<ApplicationUser> GetBannedProfile(string userId);
        Task<ApplicationUser?> RemoveAllReports(string userId);
        Task<List<ApplicationUser>> GetBannedAccounts(PageSettingsRequest model);
        Task<ApplicationUser> GetUserToChatById(string userId);
        Task<bool> SendFriendRequest(FriendRequest friendRequest);
        Task<List<FriendRequest>> GetFriendRequest(string userId);
        Task<int> GetMatchCount(string userId);
        Task<int> GetFriendCount(string userId);
        Task<bool> AcceptFriendship(string userId, int friendRequestId);
        Task<List<ApplicationUser>> GetUserMatches(PageSettingsRequest model, string? UserId, string myUserId);
        Task<List<ApplicationUser>> GetUserFriends(PageSettingsRequest model, string? UserId, string myUserId);
        Task<bool> IsFriend(string userId, string myUserId);
        Task<int> GetMutualFriendsCount(string userId, string myUserId);
    }
}
