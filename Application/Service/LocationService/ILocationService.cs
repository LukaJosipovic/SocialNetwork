using Application.DTO;
using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.LocationService
{
    public interface ILocationService
    {
        Task<GeneralResponse> AddUserLocation(string userId, double latitude, double longitude);
        List<LocationDTO> GetAllLocations(string userId);
        bool RemoveUserLocation(string userId);
    }
}
