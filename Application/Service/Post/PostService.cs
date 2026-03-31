using Application.Contracts;
using Application.DTO;
using Application.DTO.Response;
using Application.Helper;
using Application.Service.Email;
using Azure;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Post
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailService _emailService;
        private readonly ILocationRepository _locationRepository;
        public PostService(IPostRepository postRepository, IAccountRepository accountRepository, IEmailService emailService, ILocationRepository locationRepository)
        {
            _postRepository = postRepository;
            _accountRepository = accountRepository;
            _emailService = emailService;
            _locationRepository = locationRepository;
        }

        public async Task<GeneralResponse> DeletePostAdmin(int postId)
        {
            try
            {
                var result = await _postRepository.DeletePostAdmin(postId);
                if (result)
                    return ResponseHelper.CreateGeneralResponse(true, "Post deleted successfully");

                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }
        public async Task<GeneralResponse> DeletePost(string userId, int postId)
        {
            try
            {
                var post = await _postRepository.GetPostById(postId);
                if (post.UserId == userId)
                {
                    var result = await _postRepository.DeletePostAdmin(postId);
                    if (result)
                        return ResponseHelper.CreateGeneralResponse(true, "Post deleted successfully");

                    return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
                }
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<List<PostDetailsResponse>> GetAllPosts(string userId)
        {
            try
            {
                var postsDetails = new List<PostDetailsResponse>();
                var posts = await _postRepository.GetAllPosts();

                foreach (var post in posts)
                {
                    var postDetails = new PostDetailsResponse
                    {
                        PostId = post.Id,
                        Content = post.Content,
                        Description = post.Description,
                        DateCreated = post.DateCreated,
                        PostImage = post.PostImage,
                        PostImageString = post.PostImage == null ? null : $"data:image;base64,{Convert.ToBase64String(post.PostImage)}",
                        LikeCount = post.Likes == null ? 0 : post.Likes.Count(),
                        IsLiked = post.Likes == null ? false : post.Likes.Any(l => l.UserId == userId),
                        User = new DTO.UserBriefDetailsDTO
                        {
                            Id = post.User.Id,
                            Name = post.User.Name,
                            ProfilePictureString = $"data:image;base64,{Convert.ToBase64String(post.User.ProfilePicture)}"
                        },
                        IsSuccess = true
                    };
                    postsDetails.Add(postDetails);
                }
                return postsDetails;
            }
            catch (Exception)
            {
                return null;//implementirati handler za exceptione
                throw;
            }
        }

        public async Task<PostDetailsResponse> GetPostById(int id)
        {
            try
            {
                var post = await _postRepository.GetPostById(id);

                var postDetails = new PostDetailsResponse
                {
                    PostId = post.Id,
                    Content = post.Content,
                    Description = post.Description,
                    DateCreated = post.DateCreated,
                    PostImage= post.PostImage,
                    PostImageString = post.PostImage == null ? null : $"data:image;base64,{Convert.ToBase64String(post.PostImage)}",
                    User = new DTO.UserBriefDetailsDTO
                    {
                        Id = post.User.Id,
                        Name = post.User.Name,
                        ProfilePictureString = $"data:image;base64,{Convert.ToBase64String(post.User.ProfilePicture)}"
                    },
                    IsSuccess = true
                };

                return postDetails;
            }
            catch (KeyNotFoundException ex)
            {
                return new PostDetailsResponse
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<List<PostDetailsResponse>?> GetReportedPosts()
        {
            try
            {
                var postsDetails = new List<PostDetailsResponse>();
                var posts = await _postRepository.GetReportedPosts();

                foreach (var post in posts)
                {
                    var postDetails = new PostDetailsResponse
                    {
                        PostId = post.Id,
                        Content = post.Content,
                        Description = post.Description,
                        DateCreated = post.DateCreated,
                        PostImage = post.PostImage,
                        PostImageString = post.PostImage == null ? null : $"data:image;base64,{Convert.ToBase64String(post.PostImage)}",
                        User = new DTO.UserBriefDetailsDTO
                        {
                            Id = post.User.Id,
                            Name = post.User.Name,
                            ProfilePictureString = $"data:image;base64,{Convert.ToBase64String(post.User.ProfilePicture)}"
                        },
                        NumberOfReports = post.Reports.Count,
                        IsSuccess = true
                    };
                    postsDetails.Add(postDetails);
                }
                return postsDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<LikeResponse?> LikePost(int postId, string userId)
        {
            try
            {
                var user = await _accountRepository.GetUserById(userId);
                var post = await _postRepository.GetPostById(postId);
                
                var like = new Like
                {
                    UserId = user.Id,
                    PostId = postId,
                    User = user,
                    Post = post,
                };

                var result = await _postRepository.LikePost(like);
                
                if (result)
                {
                    var updatedPost = await _postRepository.GetPostById(postId);
                    
                    var likeResponse = new LikeResponse
                    {
                        IsSuccess = true,
                        LikeCount = post.Likes == null ? 0 : updatedPost.Likes.Count,
                        PostId = postId
                    };

                    return likeResponse;
                }
                
                return new LikeResponse
                {
                    IsSuccess = false,
                    Message = "Something went wrong"
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<LikeResponse> DislikePost(int postId, string userId)
        {
            try
            {
                var user = await _accountRepository.GetUserById(userId);
                var post = await _postRepository.GetPostById(postId);

                var result = await _postRepository.DislikePost(postId, userId);

                if (result)
                {
                    var updatedPost = await _postRepository.GetPostById(postId);

                    var likeResponse = new LikeResponse
                    {
                        IsSuccess = true,
                        LikeCount = post.Likes == null ? 0 : updatedPost.Likes.Count,
                        PostId = postId
                    };

                    return likeResponse;
                }

                return new LikeResponse
                {
                    IsSuccess = false,
                    Message = "Something went wrong"
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<GeneralResponse> RemoveReport(int postId)
        {
            try
            {
                var post = await _postRepository.GetPostById(postId);
                var reports = await _postRepository.GetReportPostId(postId);
                if (reports.Count > 0)
                {
                    var result = await _postRepository.RemoveReports(reports);
                    if (result)
                    {
                        var reportNumber = await _accountRepository.GetReportCount(post.UserId);

                        if (reportNumber < 1)
                        {
                            var isUnbanned = await _accountRepository.UnbanUser(post.UserId);
                            if (isUnbanned.Succeeded)
                            {
                                var email = new EmailDTO
                                {
                                    To = post.User.Email,
                                    Subject = "Your account has been unbanned",
                                    Body = "Your account has been unbanned after an admin reviewed your posts."
                                };
                                //_emailService.SendEmail(email);
                            }
                        }
                        return ResponseHelper.CreateGeneralResponse(true, "Reports have been removed from post");
                    }
                    return ResponseHelper.CreateGeneralResponse(true, "Something went wrong");
                }
                return ResponseHelper.CreateGeneralResponse(true, "The post does not have any reports");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(true, "Something went wrong");
            }
        }

        public async Task<GeneralResponse> ReportPost(int id, string userId)
        {
            try
            {
                var post = await _postRepository.GetPostById(id);
                var reporter = await _accountRepository.GetUserById(userId);
                var reportedUser = await _accountRepository.GetUserById(post.User.Id);
                var reportExists = await _accountRepository.CheckIfReportExist(reporter.Id, post.Id);

                if(reportExists)
                    return ResponseHelper.CreateGeneralResponse(false, "You have already reported this post.");

                var report = new Report
                {
                    ReporterId = userId,
                    Reporter = reporter,
                    ReportedUserId = post.User.Id,
                    ReportedUser = reportedUser,
                    ReportedPost = post
                };
                var isSuccess = await _postRepository.ReportPost(report);

                if (isSuccess)
                {
                    var reportNumber = await _accountRepository.GetReportCount(post.User.Id);

                    if (reportNumber > 0)
                    {
                        _locationRepository.RemoveUserLocation(userId);
                        var isBanned = await _accountRepository.BanAccount(post.User.Id);
                        if (isBanned.Succeeded)
                        {
                            var email = new EmailDTO
                            {
                                To = reportedUser.Email,
                                Subject = "Your account has been banned",
                                Body = "Your account has been banned due to too many reports to your account. If you think this ban is unjustified, contact our admin"
                            };
                            //_emailService.SendEmail(email);
                        }
                    }
                    return ResponseHelper.CreateGeneralResponse(true, $"You reported {post.User.Name}'s post");
                }
                
                return ResponseHelper.CreateGeneralResponse(false, "Post cannot be reported");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
        }
    }
}
