using Application.Contracts;
using Application.DTO.Request;
using Application.DTO.Response;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ConversationDTO>> GetUsersForChat(string userId, PageSettingsRequest model)
        {
            var skip = (model.PageNumber - 1) * model.PageSize;
            var conversations = _context.Conversation.IgnoreQueryFilters().Where(c => c.User1Id == userId || c.User2Id == userId);

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                conversations = conversations.Where(c => 
                    (c.User1Id == userId && c.User2.Name.Contains(model.SearchTerm)) || 
                    (c.User2Id == userId && c.User1.Name.Contains(model.SearchTerm)));
            }

            var result = await conversations
                .OrderByDescending(c => c.LastMessageAt)
                .Skip(skip)
                .Take(model.PageSize)
                .Select(c => new ConversationDTO
                {
                    ConversationId = c.Id,
                    UserId = c.User1Id == userId ? c.User2Id : c.User1Id,
                    Name = c.User1Id == userId ? c.User2.Name : c.User1.Name,
                    ProfilePicture = c.User1Id == userId ? c.User2.ProfilePicture : c.User1.ProfilePicture,
                    HasUnreadMessages = _context.Set<ChatMessage>().Any(m => m.ConversationId == c.Id && !m.IsRead && m.UserId != userId),
                    IsBlocked = c.IsBlocked
                }).ToListAsync();

            return result;
        }
        public async Task<List<ConversationDTO>> GetActivityChat(string userId, int activityId, PageSettingsRequest model)
        {
            var skip = (model.PageNumber - 1) * model.PageSize;

            var matches = await _context.Match
                .Where(m => m.CreatorId == userId && m.Activity.Id == activityId)
                .Select(m => new
                {
                    User1Id = string.Compare(userId, m.AcceptorId) < 0 ? userId : m.AcceptorId,
                    User2Id = string.Compare(userId, m.AcceptorId) < 0 ? m.AcceptorId : userId
                })
                .ToListAsync();

            var userPairs = matches.Select(m => new { m.User1Id, m.User2Id }).ToList();

            var conversations = await (
                from c in _context.Conversation
                join m in _context.Match
                on new { A = c.User1Id, B = c.User2Id }
                equals new
                {
                    A = string.Compare(userId, m.AcceptorId) < 0 ? userId : m.AcceptorId,
                    B = string.Compare(userId, m.AcceptorId) < 0 ? m.AcceptorId : userId
                }
                where m.CreatorId == userId && m.Activity.Id == activityId
                select new ConversationDTO
                {
                    ConversationId = c.Id,
                    UserId = c.User1Id == userId ? c.User2Id : c.User1Id,
                    Name = c.User1Id == userId ? c.User2.Name : c.User1.Name,
                    ProfilePicture = c.User1Id == userId ? c.User2.ProfilePicture : c.User1.ProfilePicture,
                    HasUnreadMessages = _context.Set<ChatMessage>()
                    .Any(msg => msg.ConversationId == c.Id && !msg.IsRead && msg.UserId != userId),
                }
                ).Skip(skip).Take(model.PageSize).ToListAsync();

            return conversations;
        }

        public async Task<List<ChatMessage>> GetMessages(string userId, int conversationId, PageSettingsRequest model)
        {
            var skip = (model.PageNumber - 1) * model.PageSize;

            //return await _context.ChatMessage.IgnoreQueryFilters().Where(m => m.ConversationId == conversationId).OrderBy(m => m.Timestamp).Skip(skip).Take(model.PageSize).ToListAsync();
            return await _context.ChatMessage
                .IgnoreQueryFilters()
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.Timestamp)
                .Skip(skip)
                .Take(model.PageSize)
                .OrderBy(m => m.Timestamp).ToListAsync();
        }

        public async Task<bool> MarkMessagesAsRead(string userId, int conversationId)
        {
            var unreadMessages = await _context.ChatMessage.IgnoreQueryFilters().Where(m => m.ConversationId == conversationId && m.UserId != userId && !m.IsRead) .ToListAsync();

            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;

            return false;
        }

        public async Task<bool> CheckIfUserIsBlocked(string userId, string userToChatId)
        {
            return await _context.UserBlocks.AnyAsync(u => (u.BlockedUserId == userId && u.BlockerUserId == userToChatId) || (u.BlockedUserId == userToChatId && u.BlockerUserId == userId));
        }

        public async Task<bool> SaveMessage(ChatMessage message, string user1Id, string user2Id)
        {
            await _context.ChatMessage.AddAsync(message);
            var conversation = await _context.Conversation.FirstOrDefaultAsync(c => c.User1Id == user1Id && c.User2Id == user2Id) ?? throw new KeyNotFoundException("Conversation not found");
            conversation.LastMessageAt = DateTime.Now;

            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;
            return false;
        }
    }
}
