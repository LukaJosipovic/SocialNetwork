using Application.Contracts;
using Application.Service.Activity;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.Repository.Helper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private static readonly ConcurrentDictionary<string, bool> _activityKeys = new();

        public ActivityRepository(AppDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public bool CreateActivity(ActivityCache model)
        {
            string cacheKey = Guid.NewGuid().ToString();
            model.CacheKey = cacheKey;
            _memoryCache.Set(cacheKey, model, TimeSpan.FromHours(1));
            return _activityKeys.TryAdd(cacheKey, true);
        }

        public List<ActivityCache> GetActivities(double latitude, double longitude, string userId)
        {
            var activities = new List<ActivityCache>();
            var expiredKeys = new List<string>();

            foreach (var key in _activityKeys.Keys)
            {
                if (_memoryCache.TryGetValue<ActivityCache>(key, out var activity))
                {
                    double distance = LocationHelper.GetDistanceInKm(latitude, longitude, activity.Latitude, activity.Longitude);

                    if (distance <= activity.Range && activity.UserId != userId)
                        activities.Add(activity);
                }
                else
                    expiredKeys.Add(key);

                if (expiredKeys.Count > 0)
                {
                    foreach (var expiredKey in expiredKeys)
                    {
                        _activityKeys.TryRemove(expiredKey, out _);
                    }
                }
            }
            return activities;
        }

        public ActivityCache GetActivityByCacheKey(string cacheKey)
        {
            if (_memoryCache.TryGetValue<ActivityCache>(cacheKey, out var activity))
                return activity;
            
            return null;
        }

        public async Task<bool> CreateMatch(Match match)
        {
            await _context.Match.AddAsync(match);
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
                return true;

            return false;
        }

        public async Task<bool> SaveActivity(Domain.Model.Activity activity)
        {
            await _context.Activity.AddAsync(activity);
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
                return true;

            return false;
        }
    }
}
