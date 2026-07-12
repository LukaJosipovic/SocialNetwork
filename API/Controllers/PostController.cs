using Application.DTO.Request;
using Application.Service.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("GetPostById")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _postService.GetPostById(userId, id);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts([FromQuery] PageSettingsRequest model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _postService.GetAllPosts(userId, model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("ReportPost")]
        public async Task<IActionResult> ReportPost(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _postService.ReportPost(id, userId);

            if (result.IsSuccess) 
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("LikePost")]
        public async Task<IActionResult> LikePost(int postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _postService.LikePost(postId, userId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("DislikePost")]
        public async Task<IActionResult> DislikePost(int postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _postService.DislikePost(postId, userId);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("DeletePost")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _postService.DeletePost(userId, postId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetReportedPosts")]
        public async Task<IActionResult> GetReportedPosts()
        {
            var result = await _postService.GetReportedPosts();

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletePostAdmin")]
        public async Task<IActionResult> DeletePostAdmin(int postId)
        {
            var result = await _postService.DeletePostAdmin(postId);
            
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("RemoveReport")]
        public async Task<IActionResult> RemoveReport(int postId)
        {
            var result = await _postService.RemoveReport(postId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
