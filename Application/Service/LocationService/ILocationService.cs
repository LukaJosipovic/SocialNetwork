using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.LocationService
{
    public interface ILocationService
    {
        Task<bool> AddUserLocation(string userId, double latitude, double longitude);
        List<LocationDTO> GetAllLocations(string userId);
        bool RemoveUserLocation(string userId);
    }
}
