using Application.DTO.Request;
using Application.DTO.Response;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IChatRepository
    {
        Task<List<ConversationDTO>> GetUsersForChat(string userId, PageSettingsRequest model);
        Task<List<ChatMessage>> GetMessages(string userId, int conversationId, PageSettingsRequest model);
        Task<bool> MarkMessagesAsRead(string userId, int conversationId);
        Task<bool> CheckIfUserIsBlocked(string userId, string userToChatId);
        Task<bool> SaveMessage(ChatMessage message, string user1Id, string user2Id);
        Task<List<ConversationDTO>> GetActivityChat(string userId, int activityId, PageSettingsRequest model);
    }
}
