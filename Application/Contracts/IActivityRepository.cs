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
        List<ActivityCache> GetActivities(double latitude, double longitude, string userId);
        ActivityCache GetActivityByCacheKey(string cacheKey);
        Task<bool> SaveActivity(Activity activity);
    }
}
