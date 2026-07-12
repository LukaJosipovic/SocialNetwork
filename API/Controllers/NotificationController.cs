using Application.Contracts;
using Application.Service.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("RegisterDevice")]
        public async Task<IActionResult> RegisterDevice(string deviceToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _notificationService.RegisterDevice(userId, deviceToken);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);  
        }

        [HttpPut("DeactivateDevice")]
        public async Task<IActionResult> DeactivateDevice(string deviceToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _notificationService.DeactivateDevice(userId, deviceToken);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);  
        }
    }
}
