using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ChatRoomResponse> GetChatRooms(PageSettingsRequest model)
        {   
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.GetAsync($"api/Chat/GetChatRooms?PageNumber={model.PageNumber}&PageSize={model.PageSize}");
            var responseObject = await response.Content.ReadFromJsonAsync<ChatRoomResponse>();
            return responseObject;
        }

        public async Task<List<ChatMessage>> GetMessages()
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.GetAsync($"api/Chat/GetMessages");
            var responseObject = await response.Content.ReadFromJsonAsync<List<ChatMessage>>();
            return responseObject;
        }
    }
}
