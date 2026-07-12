using Application.Contracts;
using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Helper;
using Application.Service.Notification;
using Domain.Model;
using FirebaseAdmin.Messaging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Esf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Activity
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly INotificationService _notificationService;
        private readonly ILocationRepository _locationRepository;

        public ActivityService(IActivityRepository activityRepository, IAccountRepository accountRepository, ILocationRepository locationRepository, INotificationService notificationService)
        {
            _activityRepository = activityRepository;
            _accountRepository = accountRepository;
            _locationRepository = locationRepository;
            _notificationService = notificationService;
        }

        public async Task<GeneralResponse> AcceptActivity(string cacheKey, string userId)
        {
            try
            {
                var activityCache = _activityRepository.GetActivityByCacheKey(cacheKey);
                activityCache.AcceptorIds.Add(userId);
                var creator = await _accountRepository.GetUserById(activityCache.UserId);
                var acceptor = await _accountRepository.GetUserById(userId);

                if (activityCache != null)
                {
                    var activity = new Domain.Model.Activity
                    {
                        Description = activityCache.Description,
                        ActivityCategory = activityCache.ActivityCategory,
                        User = creator
                    };

                    var match = new Match
                    {
                        CreatorId = creator.Id,
                        AcceptorId = userId,
                        DateMatched = DateTime.Now,
                        Creator = creator,
                        Acceptor = acceptor,
                        Activity = activity
                    };

                    //var activityCreated = _activityRepository.CreateActivity(activityCache);
                    var activitySaved = await _activityRepository.SaveActivity(activity);
                    var matchCreated = await _activityRepository.CreateMatch(match);

                    if (activitySaved == true && matchCreated == true)
                    {
                        var user1Id = creator.Id.CompareTo(acceptor.Id) < 0 ? creator.Id : acceptor.Id;
                        var user2Id = creator.Id.CompareTo(acceptor.Id) < 0 ? acceptor.Id : creator.Id;

                        var conversation = new Conversation
                        {
                            User1Id = user1Id,
                            User2Id = user2Id,
                            LastMessageAt = DateTime.Now,
                        };
                        var conversationCreated = await _activityRepository.CreateConversation(conversation);
                        if (conversationCreated == true)
                        {
                            var deviceTokens = await _notificationService.GetDeviceToken(activityCache.UserId);
                            if (deviceTokens != null && deviceTokens.Any())
                            {
                                await NotificationHelper.SendNotifications(deviceTokens, "Activity accepted", $"{acceptor.Name} accepted your activity");
                            }
                            return ResponseHelper.CreateGeneralResponse(true, "You accepted activity");
                        }
                    }
                }
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                return ResponseHelper.CreateGeneralResponse(true, "You accepted activity");
            }
            catch (Exception ex) 
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<GeneralResponse> CreateActivity(CreateActivityRequest request, string userId)
        {
            try
            {
                var user = await _accountRepository.GetUserById(userId);
                var activity = new ActivityCache
                {
                    Description = request.Description,
                    ActivityCategory = request.Category,
                    UserId = userId,
                    Range = request.Range,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude
                };

                var activityCreated = _activityRepository.CreateActivity(activity);

                if (activityCreated)
                {
                    var IdRange = _locationRepository.GetUserIdsByLocations(request.Latitude, request.Longitude, request.Range);
                    var deviceTokens = await _accountRepository.GetDeviceTokensByIdRange(IdRange, request.Category);

                    if (deviceTokens.Count > 0)
                        await NotificationHelper.SendNotifications(deviceTokens, "New Activity", "New activities in your area");

                    return ResponseHelper.CreateGeneralResponse(true, "Activity created succesfully");
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

        public async Task<ActivityResponse> GetActivities(double latitude, double longitude, string userId)
        {
            try
            {
                var result = _activityRepository.GetActivities(latitude, longitude, userId);
                var activitiesList = new List<ActivityDTO>();
                var user = await _accountRepository.GetUserById(userId);
                foreach (var activity in result)
                {
                    if (user.Activities != null && !user.Activities.Contains(activity.ActivityCategory))
                        continue;

                    var activityUser = await _accountRepository.GetUserById(activity.UserId);
                    var activityResponse = new ActivityDTO
                    {
                        CacheKey = activity.CacheKey,
                        Description = activity.Description,
                        ActivityCategory = activity.ActivityCategory,
                        User = new UserBriefDetailsDTO
                        {
                            Id = activity.UserId,
                            Name = activityUser.Name,
                            ProfilePictureString = $"data:image;base64,{Convert.ToBase64String(activityUser.ProfilePicture)}",
                        }
                    };
                    activitiesList.Add(activityResponse);
                }
                return ResponseHelper.CreateActivityResponse(true, null, activitiesList);
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateActivityResponse(false, "Something went wrong", null);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateActivityResponse(false, "Something went wrong", null);
            }
        }

        public async Task<MyActivityResponse> GetMyActivities(string userId, PageSettingsRequest model)
        {
            try
            {
                var myActivityResponse = new MyActivityResponse();

                var result = await _activityRepository.GetMyActivities(userId, model);

                return ResponseHelper.CreateMyActivityResponse(true, null, result);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateMyActivityResponse(false, "Somethinbg went wrong", null);
            }
        }
        public async Task<ActivityResponse> GetMyAcceptedActivities(string userId, PageSettingsRequest model)
        {
            try
            {
                var acceptedAcvitivies = new List<ActivityDTO>();

                var result = await _activityRepository.GetMyAcceptedMatches(userId, model);

                foreach (var match in result)
                {
                    var activity = new ActivityDTO
                    {
                        Description = match.Activity.Description,
                        ActivityCategory = match.Activity.ActivityCategory,
                        User = new UserBriefDetailsDTO
                        {
                            Id = match.CreatorId,
                            Name = match.Creator.Name,
                            ProfilePictureString = $"data:image;base64,{Convert.ToBase64String(match.Creator.ProfilePicture)}",
                        }
                    };
                    acceptedAcvitivies.Add(activity);
                }

                return ResponseHelper.CreateActivityResponse(true, null, acceptedAcvitivies);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateActivityResponse(false, "Somethinbg went wrong", null);
            }
        }

        public async Task<List<string>> FilterUserIdRange(CreateActivityRequest request, string userId)
        {
            var IdRange = _locationRepository.GetUserIdsByLocations(request.Latitude, request.Longitude, request.Range);
            var filteredIdRange = await _accountRepository.FilterUserIdRange(IdRange, request.Category, userId);
            return filteredIdRange;
        }
    }
}
