using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using Application.Helper;
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
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Chat/GetChatRooms?PageNumber={model.PageNumber}&PageSize={model.PageSize}&SearchTerm={model.SearchTerm}");
                var responseObject = await response.Content.ReadFromJsonAsync<ChatRoomResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateChatRoomResponse(false, ex.Message);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreateChatRoomResponse(false, "Something went wrong");
            }
        }
        public async Task<ChatRoomResponse> GetActivityChat(int activityId, PageSettingsRequest model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Chat/GetActivityChat?activityId={activityId}&PageNumber={model.PageNumber}&PageSize={model.PageSize}");
                var responseObject = await response.Content.ReadFromJsonAsync<ChatRoomResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateChatRoomResponse(false, ex.Message);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreateChatRoomResponse(false, "Something went wrong");
            }
        }

        public async Task<MessageResponse> GetMessages(string recipientId, int conversationId, PageSettingsRequest model)
        {
            try
            {
                model.PageSize = 20;
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Chat/GetMessages?userToChatId={recipientId}&conversationId={conversationId}&PageNumber={model.PageNumber}&PageSize={model.PageSize}");
                var responseObject = await response.Content.ReadFromJsonAsync<MessageResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateMessageResponse(false, ex.Message);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreateMessageResponse(false, "Something went wrong");
            }
        }
    }
}
