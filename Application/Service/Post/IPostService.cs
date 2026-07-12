using Application.DTO.Request;
using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Post
{
    public interface IPostService
    {
        Task<PostDetailsDTO> GetPostById(string userId, int id);
        Task<PostDetailsResponse> GetAllPosts(string userId, PageSettingsRequest model);
        Task<GeneralResponse> ReportPost(int id, string userId);
        Task<PostDetailsResponse> GetReportedPosts();
        Task<GeneralResponse> DeletePostAdmin(int postId);
        Task<GeneralResponse> DeletePost(string userId, int postId);
        Task<GeneralResponse> RemoveReport(int postId);
        Task<LikeResponse?> LikePost(int postId, string userId);
        Task<LikeResponse> DislikePost(int postId, string userId);
    }
}
