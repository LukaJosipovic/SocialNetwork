using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Domain.Model;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Chat
{
    public interface IChatService
    {
        Task<ChatRoomResponse> GetChatRooms(string userId, PageSettingsRequest model);
        Task<MessageResponse> GetMessages(string userId, string userToChatId);
    }
}
