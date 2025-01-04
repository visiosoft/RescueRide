using RescueRide.DTOs.GuestDtos;
using RescueRide.Services;
using Microsoft.AspNetCore.Mvc;

namespace RescueRide.Controllers
{
    [ApiController]
    [Route("api/guests")]
    public class GuestController : ControllerBase
    {
        private readonly IGuestUserService _guestUserService;

        public GuestController(IGuestUserService guestUserService)
        {
            _guestUserService = guestUserService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGuestUser([FromBody] GuestUserDTO guestUserDto)
        {
            var guestUserId = await _guestUserService.CreateGuestUserAsync(guestUserDto);
            return Ok(new { GuestUserId = guestUserId, Message = "Guest user created successfully." });
        }
    }
}
