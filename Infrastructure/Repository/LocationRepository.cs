using Application.Contracts;
using Application.DTO;
using Infrastructure.Repository.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IMemoryCache _memoryCache;
        private static readonly ConcurrentDictionary<string, bool> _locationKeys = new();

        public LocationRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool AddUserLocation(LocationDTO location)
        {
            _memoryCache.Set(location.Id, location);
            _locationKeys.TryAdd(location.Id, true);

            if (_memoryCache.TryGetValue<LocationDTO>(location.Id, out _))
                return true;
            else
                return false;
        }
        public bool UpdateGhostModeSettings(string userId, bool ghostMode)
        {
            if (_memoryCache.TryGetValue<LocationDTO>(userId, out var location))
            {
                location.GhostMode = ghostMode;
                _memoryCache.Set(location.Id, location);
            }

            if (_memoryCache.TryGetValue<LocationDTO>(location.Id, out _))
                return true;
            else
                return false;
        }
        public bool UpdateDoNotDisturbSettings(string userId, bool doNotDisturb)
        {
            if (_memoryCache.TryGetValue<LocationDTO>(userId, out var location))
            {
                location.DoNotDisturb = doNotDisturb;
                _memoryCache.Set(location.Id, location);
            }

            if (_memoryCache.TryGetValue<LocationDTO>(location.Id, out _))
                return true;
            else
                return false;
        }

        public bool RemoveUserLocation(string userId)
        {
            _memoryCache.Remove(userId);
            return _locationKeys.TryRemove(userId, out _);
        }

        public List<LocationDTO> GetAllLocations()
        {
            var locations = new List<LocationDTO>();

            foreach (var key in _locationKeys.Keys)
            {
                if (_memoryCache.TryGetValue<LocationDTO>(key, out var location))
                {
                    if (location.GhostMode == false)
                        locations.Add(location);
                }
            }
            return locations;
        }

        public List<string> GetUserIdsByLocations(double latitude, double longitude, int range)
        {
            var usersIds = new List<string>();
            //var expiredKeys = new List<string>();

            foreach (var key in _locationKeys.Keys)
            {
                if (_memoryCache.TryGetValue<LocationDTO>(key, out var location))
                {
                    if (location.DoNotDisturb)
                        continue;

                    double distance = LocationHelper.GetDistanceInKm(latitude, longitude, location.Latitude, location.Longitude);

                    if (distance <= range)
                        usersIds.Add(location.Id);
                }
                //else
                //    expiredKeys.Add(key);

                //if (expiredKeys.Count > 0)
                //{
                //    foreach (var expiredKey in expiredKeys)
                //    {
                //        _locationKeys.TryRemove(expiredKey, out _);
                //    }
                //}
            }
            return usersIds;
        }
    }
}
