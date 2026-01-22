using Application.Contracts;
using Application.DTO.Request;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Conversation>> GetUsersForChat(string userId, PageSettingsRequest model)
        {
            var skip = (model.PageNumber - 1) * model.PageSize;

            var conversations = await _context.Conversation.Where(c => c.User1Id == userId || c.User2Id == userId)
                .Include(c => c.Messages)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Skip(skip)
                .Take(model.PageSize)
                .ToListAsync();

            foreach (var conversation in conversations)
            {
                conversation.HasUnreadMessages = conversation.Messages != null && conversation.Messages.Any(m => !m.IsRead && m.UserId != userId);
            }
            return conversations;
        }

        public async Task<List<ChatMessage>> GetMessages(string userId, int conversationId)
        {
            return await _context.ChatMessage.IgnoreQueryFilters().Where(m => m.ConversationId == conversationId).OrderBy(m => m.Timestamp).ToListAsync();
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

        public async Task<bool> SaveMessage(ChatMessage message)
        {
            await _context.ChatMessage.AddAsync(message);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;
            return false;
        }
    }
}
