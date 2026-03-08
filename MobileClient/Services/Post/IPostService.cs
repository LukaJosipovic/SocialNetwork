using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Post
{
    public interface IPostService
    {
        Task<PostDetailsResponse> GetPostById(int id);
        Task<List<PostDetailsResponse>> GetAllPosts();
        Task<GeneralResponse> ReportPost(int id);
        Task<List<PostDetailsResponse>> GetReportedPosts();
        Task<GeneralResponse> DeletePostAdmin(int id);
        Task<GeneralResponse> DeletePost(int id);
        Task<GeneralResponse> RemoveReport(int id);
        Task<LikeResponse> LikePost(int postId);
        Task<LikeResponse> DislikePost(int postId);
    }
}
