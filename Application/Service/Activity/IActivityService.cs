using Application.DTO.Request;
using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Activity
{
    public interface IActivityService
    {
        Task<GeneralResponse> AcceptActivity(string cacheKey, string userId);
        Task<GeneralResponse> CreateActivity(CreateActivityRequest request, string userId);
        Task<List<ActivityResponse>> GetActivities(double latitude, double longitude);
    }
}
