using RescueRide.Application.DTOs.GuestDTOs;

namespace RescueRide.Infrastructure.Repositories
{
    public interface IGuestUserRepository
    {
        Task<int> CreateGuestUserAsync(GuestUserDTO guestUserDto);
    }
}
