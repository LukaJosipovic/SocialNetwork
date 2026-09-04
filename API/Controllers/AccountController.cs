using Application.DTO;
using Application.DTO.Request;
using Application.Enum;
using Application.Service.Account;
using Domain.Model;
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
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("GetUserDetails")]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

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
                return Unauthorized("User cannot be found");

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
                return Unauthorized("User cannot be found");

            var result = await _accountService.UpdateUsername(username, userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("UpdateDescription")]
        public async Task<IActionResult> UpdateDescription(string description)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.UpdateDescription(description, userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

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
                return Unauthorized("User cannot be found");

            var result = await _accountService.CreatePost(request, userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("MyProfile")]
        public async Task<IActionResult> MyProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.GetUserProfile(userId, null);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var myUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (myUserId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.GetUserProfile(userId, myUserId);

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
                return Unauthorized("User cannot be found");

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
                return Unauthorized("User cannot be found");

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
                return Unauthorized("User cannot be found");

            var result = await _accountService.DoNotDisturb(doNotDisturb, userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("ReportUser")]
        public async Task<IActionResult> ReportUser(string userId)
        {
            var reporterId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (reporterId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.ReportUser(userId, reporterId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("UnbanUser")]
        public async Task<IActionResult> UnbanUser(string userId)
        {
            var reporterId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (reporterId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.UnbanUser(userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("BlockUser")]
        public async Task<IActionResult> BlockUser(string userIdToBlock)
        {
            var blockerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (blockerId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.BlockUser(blockerId, userIdToBlock);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("UnblockUser")]
        public async Task<IActionResult> UnblockUser(string blockedUserId)
        {
            var blockerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (blockerId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.UnblockUser(blockerId, blockedUserId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetBannedProfile")]
        public async Task<IActionResult> GetBannedProfile(string userId)
        {
            var result = await _accountService.GetBannedProfile(userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetBannedAccounts")]
        public async Task<IActionResult> GetBannedAccounts([FromQuery] PageSettingsRequest model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.GetBannedAccounts(model);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetBlockedUsers")]
        public async Task<IActionResult> GetBlockedUsers([FromQuery] PageSettingsRequest model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var blockedUsers = await _accountService.GetBlockedUsers(userId, model);

            if (blockedUsers.IsSuccess)
                return Ok(blockedUsers);

            return BadRequest(blockedUsers);
        }

        [HttpPut("Logout")]
        public async Task<IActionResult> Logout(LogoutRequest model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User cannot be found");

            model.UserId = userId;
            var result = await _accountService.Logout(model);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("SendFriendRequest")]
        public async Task<IActionResult> SendFriendRequest(string receiverUserId)
        {
            var senderUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (senderUserId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.SendFriendRequest(senderUserId, receiverUserId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetFriendRequest")]
        public async Task<IActionResult> GetFriendRequest()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.GetFriendRequest(userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("AcceptFriendship")]
        public async Task<IActionResult> AcceptFriendship(string senderId, int friendRequestId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.AcceptFriendship(senderId, userId, friendRequestId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetMatches")]
        public async Task<IActionResult> GetMatches([FromQuery] PageSettingsRequest model, string? UserId)
        {
            var myUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (myUserId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.GetMatches(model, UserId, myUserId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetFriends")]
        public async Task<IActionResult> GetFriends([FromQuery] PageSettingsRequest model, string? UserId)
        {
            var myUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (myUserId == null)
                return Unauthorized("User cannot be found");

            var result = await _accountService.GetFriends(model, UserId, myUserId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
