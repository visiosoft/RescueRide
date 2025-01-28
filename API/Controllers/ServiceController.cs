using Microsoft.AspNetCore.Mvc;
using RescueRide.Application.Services;
using RescueRide.Core.Models;

namespace RescueRide.API.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestService([FromBody] Service service)
        {
            var serviceId = await _serviceService.RequestServiceAsync(service);
            return Ok(new { ServiceId = serviceId });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateServiceStatus(int id, [FromBody] string status)
        {
            await _serviceService.UpdateServiceStatusAsync(id, status);
            return Ok("Service status updated.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelService(int id)
        {
            await _serviceService.CancelServiceAsync(id);
            return Ok("Service cancelled.");
        }
    }
}
