using Application.DTO.Request;
using Application.DTO.Response;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IActivityRepository
    {
        bool CreateActivity(ActivityCache model);
        Task<bool> CreateMatch(Match match);
        Task<bool> CreateConversation(Conversation conversation);
        List<ActivityCache> GetActivities(double latitude, double longitude, string userId);
        ActivityCache GetActivityByCacheKey(string cacheKey);
        Task<bool> SaveActivity(Activity activity);
        Task<List<MyActivityDTO>> GetMyActivities(string userId, PageSettingsRequest model);
        Task<List<Match>> GetMyAcceptedMatches(string userId, PageSettingsRequest model);
    }
}
