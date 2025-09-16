using Application.DTO.Request;
using Application.Enum;
using Application.Service.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet("GetUserDetails")]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
                return BadRequest("User cannot be found");

            var userDetails = await _accountService.GetUserById(userId);
            
            if (!userDetails.IsSuccess) 
                return NotFound(userDetails.Message);

            return Ok(userDetails);
        }

        [HttpPut("ChangeProfilePicture")]
        public async Task<IActionResult> ChangeProfilePicture(ChangeProfilePictureRequest request)
        {
            if (!ModelState.IsValid || request.ImageData.Length == 0)
                return BadRequest("Some properties are not valid");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return BadRequest("User cannot be found");

            var result = await _accountService.ChangeProfilePicture(request, userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("UpdateUsername")]
        public async Task<IActionResult> UpdateUsername(string username)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
                return BadRequest("User cannot be found");

            var result = await _accountService.UpdateUsername(username, userId);
            
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return BadRequest("User cannot be found");

            var result = await _accountService.DeleteAccount(userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost(CreatePostRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return BadRequest("User cannot be found");

            var result = await _accountService.CreatePost(request, userId);

            if(result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("MyProfile")]
        public async Task<IActionResult> MyProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return BadRequest("User cannot be found");

            var result = await _accountService.GetUserProfile(userId);

            if (result.PostDTO.Count > 0)
            {
                var request = HttpContext.Request;
                for (int i = 0; i < result.PostDTO.Count; i++)
                {
                    result.PostDTO[i].ImageUrl = $"{request.Scheme}://{request.Host}/{result.PostDTO[i].ImageUrl}";
                    //result.PostDTO[i].ImageUrl = $"http:/10.0.2.2:5209/{result.PostDTO[i].ImageUrl}";
                }
            }

            if (result.IsSuccess)
                return Ok(result);
            
            return BadRequest(result);
        }

        [HttpGet("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            if (userId == null)
                return BadRequest("User cannot be found");

            var result = await _accountService.GetUserProfile(userId);

            if (result.PostDTO.Count > 0)
            {
                var request = HttpContext.Request;
                for (int i = 0; i < result.PostDTO.Count; i++)
                {
                    result.PostDTO[i].ImageUrl = $"{request.Scheme}://{request.Host}/{result.PostDTO[i].ImageUrl}";
                }
            }

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _accountService.GetUsers();
            return Ok(users);
        }

        [HttpPut("ChangeActivities")]
        public async Task<IActionResult> ChangeActivities(List<ActivityCategory> activities)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return BadRequest("User cannot be found");

            var result = await _accountService.ChangeActivities(activities, userId);
            
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
        [HttpPut("GhostMode")]
        public async Task<IActionResult> GhostMode(bool ghostMode)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return BadRequest("User cannot be found");

            var result = await _accountService.GhostMode(ghostMode, userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
        
        [HttpPut("DoNotDisturb")]
        public async Task<IActionResult> DoNotDisturb(bool doNotDisturb)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return BadRequest("User cannot be found");

            var result = await _accountService.DoNotDisturb(doNotDisturb, userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("ReportUser")]
        public async Task<IActionResult> ReportUser(string userId)
        {
            var reporterId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _accountService.ReportUser(userId, reporterId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
