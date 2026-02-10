using Application.Contracts;
using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Enum;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public AccountRepository(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> ChangeActivities(List<string> activities, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
            user.Activities = activities;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangeProfilePicture(ChangeProfilePictureRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
            user.ProfilePicture = request.ImageData;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<bool> CreatePost(Post request, byte[]? imageData)
        {
            await _context.Post.AddAsync(request);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;

            return false;
        }


        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
        }

        public async Task<ApplicationUser> GetUserProfile(string userId)
        {
            return await _context.Users.Include(u => u.Posts)
                .ThenInclude(p => p.Likes).Include(u => u.Matches)
                .FirstOrDefaultAsync(u => u.Id == userId) ?? throw new KeyNotFoundException("User cannot be found");
        }

        public async Task<List<ApplicationUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityResult> DoNotDisturb(bool doNotDisturb, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
            user.DoNotDisturb = doNotDisturb;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> GhostMode(bool ghostMode, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
            user.GhostMode = ghostMode;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateUsername(string username, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
            user.Name = username;
            return await _userManager.UpdateAsync(user);
        }
        
        public async Task<IdentityResult> DeleteAccount(string userId, byte[] imageByte)
        {
            var user = await _userManager.Users.Include(u => u.Posts).Include(u => u.Reports).FirstOrDefaultAsync(u => u.Id == userId) ?? throw new KeyNotFoundException("User not found");

            if (user.Posts != null)
                _context.Post.RemoveRange(user.Posts);

            if (user.Reports != null)
                _context.Report.RemoveRange(user.Reports);

            await _context.SaveChangesAsync();

            user.IsDeleted = true;
            user.ProfilePicture = imageByte;
            user.Name = "Unknown User";
            user.Email = null;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<bool> ReportUser(Report report)
        {
            await _context.Report.AddAsync(report);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;

            return false;
        }

        public async Task<bool> CheckIfReportExist(string reporterId, int postId)
        {
            var report = await _context.Report.FirstOrDefaultAsync(r => r.ReporterId == reporterId && r.ReportedPost.Id == postId);
            
            if (report == null)
                return false;
            else
                return true;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email) ?? throw new KeyNotFoundException("User not found");
        }

        public async Task<bool> CheckIfUserIsReported(string reporterId, string userId)
        {
            var report = await _context.Report.FirstOrDefaultAsync(r => r.ReporterId == reporterId && r.ReportedUserId == userId);

            if (report == null)
                return false;
            else
                return true;
        }

        public async Task<IdentityResult> BanAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
            user.IsBanned = true;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<int> GetReportCount(string userId)
        {
            return await _context.Report.CountAsync(r => r.ReportedUserId == userId);
        }

        public async Task<ApplicationUser> GetBannedUser(string email)
        {
            return await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Email == email) ?? throw new KeyNotFoundException("User not found");
        }

        public async Task<IdentityResult> UnbanUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
            user.IsBanned = false;
            return await _userManager.UpdateAsync(user);
        }
        public async Task<ApplicationUser?> RemoveAllReports(string userId)
        {
            var user = await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == userId) ?? throw new KeyNotFoundException("User not found");
            user.IsBanned = false;

            var reposrts = await _context.Report.Where(r => r.ReportedUserId == userId).ToListAsync();
            _context.RemoveRange(reposrts);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return user;
            
            return null;
        }

        public async Task<bool> BlockUser(UserBlocks block)
        {
            var conversation = await _context.Conversation.FirstOrDefaultAsync(u => 
            (u.User1Id == block.BlockerUserId && u.User2Id == block.BlockedUserId) || 
            (u.User1Id == block.BlockedUserId && u.User2Id == block.BlockerUserId)) ?? throw new KeyNotFoundException("Conversation not found");
            
            conversation.IsBlocked = true;

            await _context.UserBlocks.AddAsync(block);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;

            return false;
        }
        public async Task<bool> UnblockUser(UserBlocks block)
        {
            var conversation = await _context.Conversation.FirstOrDefaultAsync(u =>
            (u.User1Id == block.BlockerUserId && u.User2Id == block.BlockedUserId) ||
            (u.User1Id == block.BlockedUserId && u.User2Id == block.BlockerUserId)) ?? throw new KeyNotFoundException("Conversation not found");

            conversation.IsBlocked = false;

            _context.UserBlocks.Remove(block);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;

            return false;
        }

        public async Task<UserBlocks?> GetBlockedUser(string blockerId, string blockedUserId)
        {
            return await _context.UserBlocks.FirstOrDefaultAsync(u => u.BlockerUserId == blockerId && u.BlockedUserId == blockedUserId);
        }

        public async Task<List<UserBlocks>> GetAllBlockedUsers(string userId, PageSettingsRequest model)
        {
            //var skip = (model.PageNumber - 1) * model.PageSize;
            return await _context.UserBlocks.Include(u => u.BlockedUser).Where(u => u.BlockerUserId == userId).ToListAsync(); //add paggination here
        }

        public async Task<ApplicationUser> GetAnyUserById(string userId)
        {
            return await _userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == userId) ?? throw new KeyNotFoundException("User not found");
        }

        public async Task<List<string>> GetDeviceTokensByIdRange(List<string> IdRange, string category)
        {
            var users = _userManager.Users.Include(u => u.Devices).Where(u => IdRange.Contains(u.Id) && u.DoNotDisturb == false && (u.Activities == null || u.Activities.Contains(category)));
            var deviceTokens = await users.SelectMany(u => u.Devices.Where(d => d.IsActive).Select(d => d.DeviceToken)).ToListAsync();
            return deviceTokens;
        }

        public async Task<ApplicationUser> GetBannedProfile(string userId)
        {
            return await _context.Users.IgnoreQueryFilters().Include(u => u.Posts)
                .ThenInclude(p => p.Likes).Include(u => u.Matches)
                .FirstOrDefaultAsync(u => u.Id == userId) ?? throw new KeyNotFoundException("User cannot be found");
        }
    }
}
