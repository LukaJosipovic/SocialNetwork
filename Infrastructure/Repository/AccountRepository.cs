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
        public async Task<ApplicationUser> GetUserToChatById(string userId)
        {
            return await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == userId) ?? throw new KeyNotFoundException("User not found"); ;
        }

        public async Task<ApplicationUser> GetUserProfile(string userId)
        {
            return await _context.Users.Include(u => u.Posts).ThenInclude(p => p.Likes).FirstOrDefaultAsync(u => u.Id == userId) ?? throw new KeyNotFoundException("User cannot be found");
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

        public async Task<IdentityResult> UpdateDescription(string description, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
            user.Description = description;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAccount(string userId, byte[] imageByte)
        {
            var user = await _userManager.Users.Include(u => u.Posts).Include(u => u.Reports).Include(u => u.Likes).FirstOrDefaultAsync(u => u.Id == userId) ?? throw new KeyNotFoundException("User not found");

            var postIds = user.Posts.Select(p => p.Id).ToList();

            var likes = await _context.Like
                .Where(l => postIds.Contains(l.PostId))
                .ToListAsync();

            _context.Like.RemoveRange(likes);

            var reports = await _context.Report
                .Where(r => postIds.Contains(r.ReportedPost.Id))
                .ToListAsync();

            _context.Report.RemoveRange(reports);

            if (user.Posts != null)
                _context.Post.RemoveRange(user.Posts);

            if (user.Reports != null)
                _context.Report.RemoveRange(user.Reports);

            if (user.Likes != null)
                _context.Like.RemoveRange(user.Likes);

            await _context.SaveChangesAsync();

            user.IsDeleted = true;
            user.ProfilePicture = imageByte;
            user.Name = "Unknown User";
            user.Email = null;
            user.UserName = "Unknown_User";
            user.NormalizedUserName = null;
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
            var skip = (model.PageNumber - 1) * model.PageSize;
            return await _context.UserBlocks.Include(u => u.BlockedUser).Where(u => u.BlockerUserId == userId).Skip(skip).Take(model.PageSize).ToListAsync();
        }

        public async Task<ApplicationUser> GetAnyUserById(string userId)
        {
            return await _userManager.Users.IgnoreQueryFilters().Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == userId) ?? throw new KeyNotFoundException("User not found");
        }

        public async Task<List<string>> GetDeviceTokensByIdRange(List<string> IdRange, string category)
        {
            var users = _userManager.Users.Include(u => u.Devices).Where(u => IdRange.Contains(u.Id) && u.DoNotDisturb == false && (u.Activities == null || u.Activities.Contains(category)));
            var deviceTokens = await users.SelectMany(u => u.Devices.Where(d => d.IsActive).Select(d => d.DeviceToken)).ToListAsync();
            return deviceTokens;
        }
        public async Task<List<string>> FilterUserIdRange(List<string> IdRange, string category, string userId)
        {
            var users = await _userManager.Users
                .Where(u => IdRange.Contains(u.Id) && u.Id != userId && u.DoNotDisturb == false && (u.Activities == null || u.Activities.Contains(category)))
                .Select(u => u.Id).ToListAsync(); 
            return users;
        }

        public async Task<ApplicationUser> GetBannedProfile(string userId)
        {
            return await _context.Users.IgnoreQueryFilters().Include(u => u.Posts)
                .ThenInclude(p => p.Likes).Include(u => u.Matches)
                .FirstOrDefaultAsync(u => u.Id == userId) ?? throw new KeyNotFoundException("User cannot be found");
        }

        public async Task<List<ApplicationUser>> GetBannedAccounts(PageSettingsRequest model)
        {
            //add pagination here
            //var skip = (model.PageNumber - 1) * model.PageSize;

            //return await _context.Users.IgnoreQueryFilters().Where(u => u.IsBanned).Skip(skip).Take(model.PageSize).ToListAsync();
            var query = _context.Users.IgnoreQueryFilters().AsQueryable();
            
            if (string.IsNullOrWhiteSpace(model.SearchTerm))
                query = query.Where(u => u.IsBanned);
            else
                query = query.Where(u => u.IsBanned && u.Email.Contains(model.SearchTerm));

            return await query.ToListAsync();
        }

        public async Task<bool> SendFriendRequest(FriendRequest friendRequest)
        {
            await _context.FriendRequests.AddAsync(friendRequest);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;

            return false;
        }

        public async Task<List<FriendRequest>> GetFriendRequest(string userId)
        {
            return await _context.FriendRequests.Include(fr => fr.Sender)
                .Where(fr => fr.ReceiverId == userId && fr.Accepted == false).OrderByDescending(fr => fr.CreatedAt).ToListAsync();
        }

        public async Task<int> GetMatchCount(string userId)
        {
            return await _context.Match.CountAsync(m => m.CreatorId == userId || m.AcceptorId == userId);
        }

        public Task<int> GetFriendCount(string userId)
        {
            return _context.FriendRequests.CountAsync(fr => (fr.SenderId == userId || fr.ReceiverId == userId) && fr.Accepted == true);
        }

        public async Task<bool> AcceptFriendship(string userId, int friendRequestId)
        {
            var friendRequest = await _context.FriendRequests.FirstOrDefaultAsync(fr => fr.Id == friendRequestId && fr.ReceiverId == userId);
            friendRequest.Accepted = true;
            
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
                return true;
            return false;
        }

        public async Task<List<ApplicationUser>> GetUserMatches(PageSettingsRequest model, string? UserId, string myUserId)
        {
            var skip = (model.PageNumber - 1) * model.PageSize;

            var targetUserId = UserId ?? myUserId;

            var query = _context.Match
                .Where(m => m.CreatorId == targetUserId || m.AcceptorId == targetUserId)
                .Select(m => new
                {
                    OtherUserId = m.CreatorId == targetUserId ? m.AcceptorId : m.CreatorId,m.DateMatched
                })
                .GroupBy(x => x.OtherUserId)
                .Select(g => new
                {
                    OtherUserId = g.Key,
                    DateMatched = g.Max(x => x.DateMatched)
                })
                .Join(
                    _context.Users,
                    match => match.OtherUserId,
                    user => user.Id,
                    (match, user) => new
                    {
                        User = user,
                        match.DateMatched
                    })
                .OrderByDescending(x => x.DateMatched)
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize);

            return await query
                .Select(x => x.User)
                .ToListAsync();

            //return users;
        }

        public async Task<List<ApplicationUser>> GetUserFriends(PageSettingsRequest model, string? UserId, string myUserId)
        {
            var skip = (model.PageNumber - 1) * model.PageSize;

            var targetUserId = UserId ?? myUserId;

            return await _context.FriendRequests
                .Where(fr => (fr.SenderId == targetUserId || fr.ReceiverId == targetUserId) && fr.Accepted)
                .OrderByDescending(fr => fr.CreatedAt)
                .Skip(skip)
                .Take(model.PageSize)
                .Select(fr => fr.SenderId == targetUserId ? fr.Receiver : fr.Sender)
                .ToListAsync();
        }

        public async Task<bool> IsFriend(string userId, string myUserId)
        {
            return await _context.FriendRequests.AnyAsync(fr => ((fr.SenderId == userId && fr.ReceiverId == myUserId) || (fr.SenderId == myUserId && fr.ReceiverId == userId)));
        }

        public async Task<int> GetMutualFriendsCount(string userId, string myUserId)
        {
            throw new NotImplementedException();
        }
    }
}
