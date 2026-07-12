using Application.DTO.Request;
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
        Task<PostDetailsDTO> GetPostById(int id);
        Task<PostDetailsResponse> GetAllPosts(PageSettingsRequest model);
        Task<GeneralResponse> ReportPost(int id);
        Task<PostDetailsResponse> GetReportedPosts();
        Task<GeneralResponse> DeletePostAdmin(int id);
        Task<GeneralResponse> DeletePost(int id);
        Task<GeneralResponse> RemoveReport(int id);
        Task<LikeResponse> LikePost(int postId);
        Task<LikeResponse> DislikePost(int postId);
    }
}
