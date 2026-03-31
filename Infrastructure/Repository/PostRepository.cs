using Application.Contracts;
using Application.DTO.Response;
using Azure.Core;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeletePostAdmin(int postId)
        {
            var post = await _context.Post.FindAsync(postId) ?? throw new KeyNotFoundException("Post cannot be found");
            
            _context.Post.Remove(post);
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
                return true;
            return false;
        }

        public async Task<bool> DislikePost(int postId, string userId)
        {
            var like = await _context.Like.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            
            _context.Like.Remove(like);
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
                return true;
            return false;
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _context.Post.Include(p => p.User).Include(p => p.Likes).OrderByDescending(p => p.DateCreated).ToListAsync();
        }

        public async Task<Post> GetPostById(int id)
        {
            return await _context.Post.IgnoreQueryFilters().Include(p => p.User).Include(p => p.Likes).FirstOrDefaultAsync(p => p.Id == id) ?? throw new KeyNotFoundException("Post cannot be found");
        }

        public async Task<List<Post>> GetReportedPosts()
        {
            return await _context.Post.IgnoreQueryFilters().Where(p => p.Reports.Count() > 0).Include(p => p.User).Include(p => p.Reports).ToListAsync();
        }

        public async Task<List<Report>> GetReportPostId(int postId)
        {
            return await _context.Report.Include(r => r.ReportedUser).IgnoreQueryFilters().Where(r => r.ReportedPost.Id == postId).ToListAsync();
        }

        public async Task<bool> LikePost(Like like)
        {
            await _context.Like.AddAsync(like);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;

            return false;
        }

        public async Task<bool> RemoveReports(List<Report> reports)
        {
            _context.RemoveRange(reports);
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
                return true;

            return false;
        }

        public async Task<bool> ReportPost(Report report)
        {
            await _context.Report.AddAsync(report);
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
                return true;

            return false;
        }
    }
}
