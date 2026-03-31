using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using Application.Service.Account;
using Application.Service.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.CreateAccount(model, "User");

                if (result.IsSuccess)
                    return Ok(result);
                
                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin(CreateAccountRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.CreateAccount(model, "Admin");

                if (result.IsSuccess)
                    return Ok(result);
                
                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Login(model);
                if (result.IsSuccess)
                    return Ok(result);

                return NotFound(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest request)
        {
            if (ModelState.IsValid)
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(request.JwtToken);
                var userId = token.Subject;

                if (userId == null)
                    return BadRequest("User cannot be found");

                request.UserId = userId;

                var result = await _authService.RefreshToken(request);
                
                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotUserPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Email address is not valid");

            var result = await _authService.ForgotPassword(request);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Some fields are not valid");

            var result = await _authService.ResetPassword(request);

            return Redirect($"/ResetPasswordSucceeded?message={result.Message}");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return BadRequest();

            var result = await _authService.ConfirmEmail(userId, token);

            return Redirect($"/ResetPasswordSucceeded?message={result.Message}");
        }
    }
}
