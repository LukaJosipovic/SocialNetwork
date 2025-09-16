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
    }
}
