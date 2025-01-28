using RescueRide.Services;
using Microsoft.AspNetCore.Mvc;

namespace RescueRide.Controllers
{
    [ApiController]
    [Route("api/providers")]
    public class ServiceProviderController : ControllerBase
    {
        private readonly IServiceProviderService _service;

        public ServiceProviderController(IServiceProviderService service)
        {
            _service = service;
        }

        [HttpGet("GetAllServicePRoviders")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _service.GetAllDriversAsync();
            return Ok(drivers);
        }

        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearbyProviders([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double radius = 10)
        {
            var providers = await _service.GetNearbyProvidersAsync(latitude, longitude, radius);
            return Ok(providers);
        }

        [HttpPut("{id}/availability")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] bool isAvailable)
        {
            await _service.UpdateAvailabilityAsync(id, isAvailable);
            return Ok("Availability updated successfully.");
        }
    }
}
