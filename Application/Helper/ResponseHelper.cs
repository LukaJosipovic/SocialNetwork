using Application.DTO;
using Application.DTO.Response;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public static class ResponseHelper
    {
        public static GeneralResponse CreateGeneralResponse(bool isSuccess, string message)
        {
            return new GeneralResponse
            {
                IsSuccess = isSuccess,
                Message = message
            };
        }

        public static ProfilePictureResponse CreateProfilePictureResponse(bool isSuccess, string? message, byte[]? imageData)
        {
            return new ProfilePictureResponse
            {
                IsSuccess = isSuccess,
                Message = message,
                ImageData = imageData
            };
        }

        public static UserDetailsResponse CreateUserDetailsResponse(ApplicationUser? user, bool isSuccess, string? message)
        {
            if (isSuccess)
            {
                return new UserDetailsResponse
                {
                    Name = user.Name,
                    Email = user.Email,
                    Description = user.Description,
                    GhostMode = user.GhostMode,
                    ProfilePicture = user.ProfilePicture,
                    IsSuccess = isSuccess
                };
            }
            else
            {
                return new UserDetailsResponse
                {
                    IsSuccess = isSuccess,
                    Message = message
                };
            }
        }

        public static UserProfileRespons CreateUserProfileRespons(ApplicationUser? user, bool isSuccess, string? message)
        {
            if (isSuccess)
            {
                return new UserProfileRespons
                {
                    Name = user.Name,
                    Description = user.Description,
                    ProfilePicture = user.ProfilePicture,
                    PostDTO = user.Posts != null ? user.Posts.Select(p => new PostDTO
                    {
                        PostID = p.Id,
                        ImageUrl = p.PostImage != null ? $"data:image;base64,{Convert.ToBase64String(p.PostImage)}" : null,
                        PostImage = p.PostImage
                    }).ToList() : new List<PostDTO>(),
                    IsSuccess = isSuccess,
                    Message = message
                };
            }else
            {
                return new UserProfileRespons
                {
                    IsSuccess = isSuccess,
                    Message = message
                };
            }
        }

        public static RegisterResponse CreateRegisterResponse(bool isSuccess, string? message, IEnumerable<string>? errors)
        {
            if (isSuccess)
            {
                return new RegisterResponse
                {
                    IsSuccess = true,
                    Message = "Registration is successful",
                };
            }
            else
            {
                return new RegisterResponse
                {
                    IsSuccess = false,
                    Message = "Registration failed",
                    Errors = errors
                };
            }
        }

        public static LoginResponse CreateLoginResponse(bool isSuccess, string? message, string? accessToken, string? userId, string? refreshToken)
        {
            if(isSuccess)
            {
                return new LoginResponse
                {
                    IsSuccess = isSuccess,
                    UserId = userId,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            else
            {
                return new LoginResponse
                {
                    IsSuccess = isSuccess,
                    Message = message
                };
            }
        }

        public static BannedAccountResponse CreateBannedUserResponse(bool IsSuccess, ApplicationUser user)
        {
            return new BannedAccountResponse
            {
                UserId = user.Id,
                Email = user.Email,
                IsSuccess = IsSuccess
            };
        }
    }
}
