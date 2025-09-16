using API.Hubs;
using Application.DTO.Request;
using Application.Service.Activity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
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
                return BadRequest("User cannot be found");

            var result = await _activityService.CreateActivity(request, userId);

            if (result.IsSuccess)
            {
                //await _hubContext.Clients.All.SendAsync("ReceiveActivityNotification");
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("GetActivities")]
        public async Task<IActionResult> GetActivities()
        {
            var result = await _activityService.GetActivities();
            return Ok(result);
        }

        [HttpPost("AcceptActivity")]
        public async Task<IActionResult> AcceptActivity([FromBody] string cacheKey)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return BadRequest("User cannot be found");

            var result = await _activityService.AcceptActivity(cacheKey, userId);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        
        [HttpGet("Match")]
        public async Task<IActionResult> GetMatch()
        {
            return Ok(await _appDbContext.Match.ToListAsync());
        }
    }
}
