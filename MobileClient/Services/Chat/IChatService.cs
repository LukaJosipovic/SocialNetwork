using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Chat
{
    public interface IChatService
    {
        Task<ChatRoomResponse> GetChatRooms(PageSettingsRequest model);
        Task<MessageResponse> GetMessages(string recipientId, int conversationId, PageSettingsRequest model);
    }
}
