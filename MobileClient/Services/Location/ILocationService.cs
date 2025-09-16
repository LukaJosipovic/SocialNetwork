using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Location
{
    public interface ILocationService
    {
        Task<List<LocationDTO>> GetUsersLocations();
        Task<bool> AddLocation(double latitude, double longitude);
        Task<bool> RemoveLocation();
    }
}
