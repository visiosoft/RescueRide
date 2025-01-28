using RescueRide.DTOs.GuestDtos;

namespace RescueRide.Repositories
{
    public interface IGuestUserRepository
    {
        Task<int> CreateGuestUserAsync(GuestUserDTO guestUserDto);
    }
}
