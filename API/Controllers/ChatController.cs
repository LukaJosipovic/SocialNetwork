using Application.DTO.Request;
using Application.DTO.Response;
using Application.Service.Chat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("GetChatRooms")]
        public async Task<IActionResult> GetChatRooms([FromQuery] PageSettingsRequest model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var chatRooms = await _chatService.GetChatRooms(userId, model);

            return Ok(chatRooms);
        }

        [HttpGet("GetMessages")]
        public async Task<IActionResult> GetMessages(string userToChatId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
                return Unauthorized("User cannot be found");

            var messages = await _chatService.GetMessages(userId, userToChatId);

            return Ok(messages);
        }
    }
}
