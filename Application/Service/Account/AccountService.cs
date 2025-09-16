using Application.Contracts;
using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Enum;
using Application.Helper;
using Application.Service.Email;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailService _emailService;

        public AccountService(IAccountRepository accountRepository, IEmailService emailService)
        {
            _accountRepository = accountRepository;
            _emailService = emailService;
        }

        public async Task<GeneralResponse> ChangeActivities(List<ActivityCategory> activities, string userId)
        {
            var activitiesString = activities.Select(x => x.ToString()).ToList();

            try
            {
                var isUpdated = await _accountRepository.ChangeActivities(activitiesString, userId);

                if (isUpdated.Succeeded)
                    return ResponseHelper.CreateGeneralResponse(true, "Activities changed successfully");

                return ResponseHelper.CreateGeneralResponse(false, "Failed to change activities");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
        }

        public async Task<ProfilePictureResponse> ChangeProfilePicture(ChangeProfilePictureRequest request, string userId)
        {
            if (request.ImageData == null || request.ImageData.Length == 0)
                return ResponseHelper.CreateProfilePictureResponse(false, "Invalid image data", null);

            try
            {
                var isUpdated = await _accountRepository.ChangeProfilePicture(request, userId);

                if (isUpdated.Succeeded)
                    return ResponseHelper.CreateProfilePictureResponse(true, "Profile picture changed successfully", request.ImageData);

                return ResponseHelper.CreateProfilePictureResponse(false, "Failed to update profile picture", null);
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateProfilePictureResponse(false, ex.Message, null);
            }
        }

        public async Task<GeneralResponse> CreatePost(CreatePostRequest request, string userId)
        {
            try
            {
                var user = await _accountRepository.GetUserById(userId);

                var post = new Domain.Model.Post
                {
                    UserId = new Guid(userId),
                    Type = (request.ImageData == null) ? PostType.Text : PostType.Image,
                    Description = request.Description,
                    DateCreated = DateTime.Now,
                    PostImage = request.ImageData,
                    User = user,
                };

                var result = await _accountRepository.CreatePost(post, request.ImageData);

                return ResponseHelper.CreateGeneralResponse(true, "Post created successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (IOException ex) 
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch(Exception)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Post cannot be created");
            }
        }

        public async Task<UserDetailsResponse> GetUserById(string userId)
        {
            try
            {
                var user = await _accountRepository.GetUserById(userId);
                return ResponseHelper.CreateUserDetailsResponse(user, true, null);
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateUserDetailsResponse(null, false, ex.Message);
            }
            catch
            {
                return ResponseHelper.CreateUserDetailsResponse(null, false, "Something went wrong");
            }
        }

        public async Task<UserProfileRespons> GetUserProfile(string userId)
        {
            try
            {
                var user = await _accountRepository.GetUserProfile(userId);

                return ResponseHelper.CreateUserProfileRespons(user, true, null);
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateUserProfileRespons(null, false, ex.Message);
            }
            catch (Exception)
            {
                return ResponseHelper.CreateUserProfileRespons(null, false, "An unexpected error occurred while getting user");
            }
        }

        public async Task<List<UserBriefDetailsDTO>> GetUsers()
        {
            var usersDetailsList = new List<UserBriefDetailsDTO>();

            var users = await _accountRepository.GetUsers();

            foreach (var user in users) 
            {
                UserBriefDetailsDTO userDetails = new UserBriefDetailsDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    ProfilePicture = user.ProfilePicture
                };
                usersDetailsList.Add(userDetails);
            }
            return usersDetailsList;
        }

        public async Task<GeneralResponse> DoNotDisturb(bool doNotDisturb, string userId)
        {
            try
            {
                var isUpdated = await _accountRepository.DoNotDisturb(doNotDisturb, userId);

                if (isUpdated.Succeeded && doNotDisturb == true)
                {
                    return ResponseHelper.CreateGeneralResponse(true, "Do not disturb option is activated");
                }
                else if (isUpdated.Succeeded && doNotDisturb == false)
                {
                    return ResponseHelper.CreateGeneralResponse(true, "Do not disturb option is deactivated");
                }
                return ResponseHelper.CreateGeneralResponse(false, "Error with do not disturb option");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
        }
        public async Task<GeneralResponse> GhostMode(bool ghostMode, string userId)
        {
            try
            {
                var isUpdated = await _accountRepository.GhostMode(ghostMode, userId);

                if (isUpdated.Succeeded && ghostMode == true)
                {
                    return ResponseHelper.CreateGeneralResponse(true, "Ghost mode is activated");
                }
                else if (isUpdated.Succeeded && ghostMode == false)
                {
                    return ResponseHelper.CreateGeneralResponse(true, "Ghost mode is deactivated");
                }
                return ResponseHelper.CreateGeneralResponse(false, "Error with ghost mode");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
        }

        public async Task<GeneralResponse> UpdateUsername(string username, string userId)
        {
            try
            {
                var isUpdated = await _accountRepository.UpdateUsername(username, userId);

                if (isUpdated.Succeeded)
                    return ResponseHelper.CreateGeneralResponse(true, "Username changed succesffully");

                return ResponseHelper.CreateGeneralResponse(false, "Username cannot be updated");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception)
            {
                return ResponseHelper.CreateGeneralResponse(false, "An unexpected error occurred while updating the username");
            }
        }

        public async Task<GeneralResponse> DeleteAccount(string userId)
        {
            try
            {
                var basePath = Path.GetDirectoryName(Environment.CurrentDirectory);
                var filePath = Path.Combine(basePath, "Img", "unknown.png");
                var imageByte = await File.ReadAllBytesAsync(filePath);

                var isDeleted = await _accountRepository.DeleteAccount(userId, imageByte);

                if (isDeleted.Succeeded)
                    return ResponseHelper.CreateGeneralResponse(true, "Account deleted succesffully");

                return ResponseHelper.CreateGeneralResponse(false, "Account cannot be deleted");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "An unexpected error occurred while deleting account");
            }
        }

        public async Task<GeneralResponse> ReportUser(string userId, string reporterId)
        {
            try
            {
                var reportedUser = await _accountRepository.GetUserById(userId);
                var reporter = await _accountRepository.GetUserById(reporterId);
                var reportExists = await _accountRepository.CheckIfUserIsReported(reporterId, userId);

                if (reportExists)
                    return ResponseHelper.CreateGeneralResponse(false, "You have already reported this user.");

                var report = new Report
                {
                    ReporterId = userId,
                    Reporter = reporter,
                    ReportedUserId = userId,
                    ReportedUser = reportedUser,
                };

                var isSuccess = await _accountRepository.ReportUser(report);

                if (isSuccess)
                {
                    var reportNumber = await _accountRepository.GetReportCount(userId);
                    
                    if (reportNumber > 1)
                    {
                        var isBanned = await _accountRepository.BanAccount(userId);
                        if (isBanned.Succeeded)
                        {
                            var email = new EmailDTO
                            {
                                To = reportedUser.Email,
                                Subject = "Your account has been banned",
                                Body = "Your account has been banned due to too many logins to your account. If you think this ban is unjustified, contact our admin"
                            };
                            _emailService.SendEmail(email);
                        }
                    }

                    return ResponseHelper.CreateGeneralResponse(true, $"You reported user {reportedUser.Name}");
                }
                return ResponseHelper.CreateGeneralResponse(false, $"User cannot be reported");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            }
            catch (Exception ex) 
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }
    }
}
