using RescueRide.DTOs.GuestDtos;

namespace RescueRide.Services
{
    public interface IGuestUserService
    {
        Task<int> CreateGuestUserAsync(GuestUserDTO guestUserDto);
    }
}
