using API.Hubs;
using Application.DTO.Request;
using Application.Service.Activity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IActivityService _activityService;
        private readonly IHubContext<ActivityHub> _hubContext;
        public ActivityController(IActivityService activityService, IHubContext<ActivityHub> hubContext, AppDbContext appDbContext)
        {
            _activityService = activityService;
            _hubContext = hubContext;
            _appDbContext = appDbContext;
        }

        [HttpPost("CreateActivity")]
        public async Task<IActionResult> CreateActivity(CreateActivityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Some fields are not valid");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _activityService.CreateActivity(request, userId);
            var targetUserIds = await _activityService.FilterUserIdRange(request, userId);
            
            await _hubContext.Clients.Users(targetUserIds).SendAsync("ReceiveActivityNotification");

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetActivities")]
        public async Task<IActionResult> GetActivities([FromQuery] double latitude, [FromQuery] double longitude)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _activityService.GetActivities(latitude, longitude, userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("AcceptActivity")]
        public async Task<IActionResult> AcceptActivity([FromBody] string cacheKey)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _activityService.AcceptActivity(cacheKey, userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
        
        [HttpGet("Match")]
        public async Task<IActionResult> GetMatch()
        {
            return Ok(await _appDbContext.Match.ToListAsync());
        }

        [HttpGet("MyActivities")]
        public async Task<IActionResult> MyActivities([FromQuery] PageSettingsRequest model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _activityService.GetMyActivities(userId, model);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
        [HttpGet("GetMyAcceptedActivities")]
        public async Task<IActionResult> GetMyAcceptedActivities([FromQuery] PageSettingsRequest model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _activityService.GetMyAcceptedActivities(userId, model);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
