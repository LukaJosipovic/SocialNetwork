using Application.Contracts;
using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Chat
{
    public class ChatService : IChatService
    {
		private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<ChatRoomResponse> GetChatRooms(string userId, PageSettingsRequest model)
        {
			try
			{
                var usersChatList = new List<UserBriefDetailsDTO>();
                
                var matches = await _chatRepository.GetUsersForChat(userId, model);

                var chatRooms = new ChatRoomResponse();
                chatRooms.MyId = userId;
                chatRooms.UsersChatList = new List<UserBriefDetailsDTO>(); ;

                foreach (var match in matches)
                {
                    if (match.AcceptorId == userId)
                    {
                        //znači da sam ja prihvatio i pokaži mi kreatora
                        UserBriefDetailsDTO userDetails = new UserBriefDetailsDTO
                        {
                            Id = match.Creator.Id,
                            Name = match.Creator.Name,
                            ProfilePicture = match.Creator.ProfilePicture
                        };
                        chatRooms.UsersChatList.Add(userDetails);
                    }
                    else if (match.CreatorId == userId)
                    {
                        //znači da sam ja kreator i prikaži mi onoga tko je prihvatio
                        UserBriefDetailsDTO userDetails = new UserBriefDetailsDTO
                        {
                            Id = match.Acceptor.Id,
                            Name = match.Acceptor.Name,
                            ProfilePicture = match.Acceptor.ProfilePicture
                        };
                        chatRooms.UsersChatList.Add(userDetails);
                    }
                }
                return chatRooms;
            }
			catch (Exception ex)
			{

				throw;
			}
        }

        public async Task<List<ChatMessage>> GetMessages(string userId)
        {
            try
            {
                return await _chatRepository.GetMessages(userId);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
