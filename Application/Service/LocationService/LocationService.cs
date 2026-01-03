using Application.Contracts;
using Application.DTO;
using Application.DTO.Response;
using Application.Helper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.LocationService
{
    public class LocationService : ILocationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILocationRepository _locationRepository;

        public LocationService(IAccountRepository accountRepository, ILocationRepository locationRepository)
        {
            _accountRepository = accountRepository;
            _locationRepository = locationRepository;
        }

        public async Task<GeneralResponse> AddUserLocation(string userId, double latitude, double longitude)
        {
            try
            {
                var user = await _accountRepository.GetUserById(userId);

                var userLocation = new LocationDTO
                {
                    Id = userId,
                    Name = user.Name,
                    ProfilePicture = user.ProfilePicture,
                    Latitude = latitude,
                    Longitude = longitude,
                    GhostMode = user.GhostMode
                };

                var result = _locationRepository.AddUserLocation(userLocation);
                if (result)
                {
                    return ResponseHelper.CreateGeneralResponse(true, "Location added succesfully");
                }
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong"); ;
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public bool RemoveUserLocation(string userId)
        {
            try
            {
                return _locationRepository.RemoveUserLocation(userId);
            }
            catch (KeyNotFoundException ey)
            {

                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<LocationDTO> GetAllLocations(string userId)
        {
            var locations = _locationRepository.GetAllLocations();
            var userLocation = locations.FirstOrDefault(u => u.Id == userId);
            if (userLocation != null)
                locations.Remove(userLocation);
            //var locationToRemove = locations.SingleOrDefault(locations => locations.Id == userId);

            //if (locationToRemove != null) 
            //    locations.Remove(locationToRemove);
            foreach (var location in locations)
            {
                if (location.ProfilePicture != null)
                {
                    location.ProfilePictureString = $"data:image;base64,{Convert.ToBase64String(location.ProfilePicture)}";
                }
            }
            
            return locations;
        }
    }
}
