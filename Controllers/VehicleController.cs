using RescueRide.Models;
using RescueRide.Services;
using Microsoft.AspNetCore.Mvc;

namespace RescueRide.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] Vehicle vehicle)
        {
            await _vehicleService.AddVehicleAsync(vehicle);
            return Ok("Vehicle added successfully.");
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetVehiclesByUserId(int id)
        {
            var vehicles = await _vehicleService.GetVehiclesByUserIdAsync(id);
            return Ok(vehicles);
        }
    }


}
