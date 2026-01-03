using Application.DTO.Request;
using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Activity
{
    public interface IActivityService
    {
        Task<GeneralResponse> CreateActivity(CreateActivityRequest request);
        Task<List<ActivityResponse>> GetActivities();
        Task<GeneralResponse> AcceptActivity(string cacheKey);
    }
}
