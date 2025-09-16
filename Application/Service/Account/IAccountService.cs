using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Enum;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Account
{
    public interface IAccountService
    {
        Task<UserDetailsResponse> GetUserById(string userId);
        Task<ProfilePictureResponse> ChangeProfilePicture(ChangeProfilePictureRequest request, string userId);
        Task<GeneralResponse> UpdateUsername(string username, string userId);
        Task<GeneralResponse> DeleteAccount(string userId);
        Task<GeneralResponse> CreatePost(CreatePostRequest request, string userId);
        Task<UserProfileRespons> GetUserProfile(string userId);
        Task<List<UserBriefDetailsDTO>> GetUsers();
        Task<GeneralResponse> ChangeActivities(List<ActivityCategory> activities, string userId);
        Task<GeneralResponse> GhostMode(bool ghostMode, string userId);
        Task<GeneralResponse> DoNotDisturb(bool doNotDisturb, string userId);
        Task<GeneralResponse> ReportUser(string userId, string reporterId);
    }
}
