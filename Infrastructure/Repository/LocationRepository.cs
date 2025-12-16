using Application.Contracts;
using Application.DTO;
using Infrastructure.Repository.Helper;
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
            _memoryCache.Set(location.Id, location, TimeSpan.FromHours(1));
            _locationKeys.TryAdd(location.Id, true);

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
            var expiredKeys = new List<string>();

            foreach (var key in _locationKeys.Keys)
            {
                if (_memoryCache.TryGetValue<LocationDTO>(key, out var location))
                {
                    if (location.GhostMode == false)
                        locations.Add(location);
                }
                else 
                    expiredKeys.Add(key);

                if (expiredKeys.Count > 0)
                {
                    foreach (var expiredKey in expiredKeys)
                    {
                        _locationKeys.TryRemove(expiredKey, out _);
                    }
                }
            }
            return locations;
        }

        public List<string> GetUserIdsByLocations(double latitude, double longitude, int range)
        {
            var usersIds = new List<string>();
            var expiredKeys = new List<string>();

            foreach (var key in _locationKeys.Keys)
            {
                if (_memoryCache.TryGetValue<LocationDTO>(key, out var location))
                {
                    double distance = LocationHelper.GetDistanceInKm(latitude, longitude, location.Latitude, location.Longitude);

                    if (distance <= range)
                        usersIds.Add(location.Id);
                }
                else
                    expiredKeys.Add(key);

                if (expiredKeys.Count > 0)
                {
                    foreach (var expiredKey in expiredKeys)
                    {
                        _locationKeys.TryRemove(expiredKey, out _);
                    }
                }
            }
            return usersIds;
        }
        //private double GetDistanceInKm(double lat1, double lon1, double lat2, double lon2)
        //{
        //    // Earth's radius in kilometers
        //    const double R = 6371;

        //    double dLat = DegreesToRadians(lat2 - lat1);
        //    double dLon = DegreesToRadians(lon2 - lon1);

        //    double a =
        //        Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
        //        Math.Cos(DegreesToRadians(lat1)) *
        //        Math.Cos(DegreesToRadians(lat2)) *
        //        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        //    double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        //    // distance in KM
        //    return R * c;
        //}

        //private double DegreesToRadians(double deg)
        //{
        //    return deg * (Math.PI / 180);
        //}
    }
}
