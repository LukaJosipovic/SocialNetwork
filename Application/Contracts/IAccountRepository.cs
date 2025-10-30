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
    }
}
