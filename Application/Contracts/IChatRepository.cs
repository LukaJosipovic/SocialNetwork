using Application.DTO.Request;
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
        Task<List<Match>> GetUsersForChat(string userId, PageSettingsRequest model);
        Task<List<ChatMessage>> GetMessages(string userId);

        Task<bool> CheckIfUserIsBlocked(string userId, string userToChatId);
    }
}
