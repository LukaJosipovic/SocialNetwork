using Application.Service.LocationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost("AddLocation")]
        public async Task<IActionResult> AddLocation(double latitude, double longitude)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                    return Unauthorized("User cannot be found");

                var result = await _locationService.AddUserLocation(userId, latitude, longitude);

                return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpDelete("RemoveLocation")]
        public async Task<IActionResult> RemoveLocation()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _locationService.RemoveUserLocation(userId);

            if (userId == null)
                return Unauthorized("User cannot be found");

            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetLocations")]
        public async Task<IActionResult> GetLocations()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //if (userId == null)
            //    return Unauthorized("User cannot be found");

            var locations = _locationService.GetAllLocations(userId);

            return Ok(locations);
        }
    }
}
