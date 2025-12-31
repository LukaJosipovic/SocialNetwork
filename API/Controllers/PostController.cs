using Application.Service.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
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
            var result = await _postService.GetPostById(id);
            //maknuti
            var request = HttpContext.Request;
            result.Content = $"{request.Scheme}://{request.Host}/{result.Content}";
            //result.Content = $"http://10.0.2.2:5209/{result.Content}";

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User cannot be found");

            var result = await _postService.GetAllPosts(userId);

            //if (result.Count > 0)
            //{
            //    //maknuti
            //    var request = HttpContext.Request;
            //    var baseUpl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            //    foreach (var post in result)
            //    {
            //        //post.Content = $"{request.Scheme}://{request.Host}/{post.Content}";
            //        post.Content = $"{baseUpl}/{post.Content}";
            //    }
            //    return Ok(result);
            //}
            return Ok(result);
            //return BadRequest(result);
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
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("DeletePost")]
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

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetReportedPosts")]
        public async Task<IActionResult> GetReportedPosts()
        {
            var result = await _postService.GetReportedPosts();
            if (result.Count > 0)
            {
                //mkanuti
                //var request = HttpContext.Request;
                //var baseUpl = $"{request.Scheme}://{request.Host}{request.PathBase}";
                //foreach (var post in result)
                //{
                //    //post.Content = $"{request.Scheme}://{request.Host}/{post.Content}";
                //    post.Content = $"{baseUpl}/{post.Content}";
                //}
                //return Ok(result);
            }
            return Ok(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("DeletePostAdmin")]
        public async Task<IActionResult> DeletePostAdmin(int postId)
        {
            var result = await _postService.DeletePostAdmin(postId);
            
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        //[Authorize(Roles = "Admin")]
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
