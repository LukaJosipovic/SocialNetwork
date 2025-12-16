using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ILocationRepository
    {
        bool AddUserLocation(LocationDTO location);
        List<LocationDTO> GetAllLocations();
        List<string> GetUserIdsByLocations(double latitude, double longitude, int range);
        bool RemoveUserLocation(string userId);
    }
}
