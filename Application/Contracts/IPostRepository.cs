using Application.DTO.Request;
using Application.DTO.Response;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IPostRepository
    {
        Task<Post> GetPostById(int id);
        Task<List<Post>> GetAllPosts(PageSettingsRequest model);
        Task<bool> ReportPost(Report report);
        Task<List<Post>> GetReportedPosts();
        Task<bool> DeletePostAdmin(int postId);
        Task<List<Report>> GetReportPostId(int postId);
        Task<bool> RemoveReports(List<Report> reports);
        Task<bool> LikePost(Like like);
        Task<bool> DislikePost(int postId, string userId);
    }
}
