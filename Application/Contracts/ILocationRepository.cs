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
        bool RemoveUserLocation(string userId);
    }
}
