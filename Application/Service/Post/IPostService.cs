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
        Task<PostDetailsResponse> GetPostById(int id);
        Task<List<PostDetailsResponse>> GetAllPosts(string userId);
        Task<GeneralResponse> ReportPost(int id, string userId);
        Task<List<PostDetailsResponse>?> GetReportedPosts();
        Task<GeneralResponse> DeletePostAdmin(int postId);
        Task<GeneralResponse> RemoveReport(int postId);
        Task<LikeResponse?> LikePost(int postId, string userId);
        Task<LikeResponse> DislikePost(int postId, string userId);
    }
}
