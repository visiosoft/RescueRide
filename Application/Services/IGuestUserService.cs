using RescueRide.Application.DTOs.GuestDTOs;

namespace RescueRide.Application.Services
{
    public interface IGuestUserService
    {
        Task<int> CreateGuestUserAsync(GuestUserDTO guestUserDto);
    }
}
