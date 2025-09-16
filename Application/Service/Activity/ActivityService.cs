using Application.Contracts;
using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Helper;
using Domain.Model;
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

        public ActivityService(IActivityRepository activityRepository, IAccountRepository accountRepository)
        {
            _activityRepository = activityRepository;
            _accountRepository = accountRepository;
        }

        public async Task<GeneralResponse> AcceptActivity(string cacheKey, string userId)
        {
            try
            {
                var activityCache = _activityRepository.GetActivityByCacheKey(cacheKey);
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

                    var activitySaved = await _activityRepository.SaveActivity(activity);
                    var matchCreated = await _activityRepository.CreateMatch(match);

                    if (activitySaved == true && matchCreated == true)
                        return ResponseHelper.CreateGeneralResponse(true, "You accepted activity");
                }
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
            catch (KeyNotFoundException ex)
            {

                throw;
            }
            catch (Exception ex) 
            {
                throw;
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
                };
                //var activity = new Domain.Model.Activity
                //{
                //    Description = request.Description,
                //    ActivityCategory = request.Category,
                //    User = user
                //};

                _activityRepository.CreateActivity(activity);

                return ResponseHelper.CreateGeneralResponse(true, "Activity created succesfully");
            }
            catch (KeyNotFoundException ex)
            {

                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ActivityResponse>> GetActivities()
        {
            try
            {
                var result = _activityRepository.GetActivities();
                var activitiesList = new List<ActivityResponse>();
                foreach (var activity in result)
                {
                    var user = await _accountRepository.GetUserById(activity.UserId);
                    var activityResponse = new ActivityResponse
                    {
                        CacheKey = activity.CacheKey,
                        Description = activity.Description,
                        ActivityCategory = activity.ActivityCategory,
                        User = new UserBriefDetailsDTO
                        {
                            Id = activity.UserId,
                            Name = user.Name,
                            ProfilePicture = user.ProfilePicture,
                        }
                    };
                    activitiesList.Add(activityResponse);
                }
                return activitiesList;
            }
            catch (KeyNotFoundException ex)
            {

                throw;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
