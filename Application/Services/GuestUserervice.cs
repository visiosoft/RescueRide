using RescueRide.Application.DTOs.GuestDTOs;
using RescueRide.Infrastructure.Repositories;

namespace RescueRide.Application.Services
{
    public class GuestUserService : IGuestUserService
    {
        private readonly IGuestUserRepository _guestUserRepository;

        public GuestUserService(IGuestUserRepository guestUserRepository)
        {
            _guestUserRepository = guestUserRepository;
        }

        public async Task<int> CreateGuestUserAsync(GuestUserDTO guestUserDto)
        {
            return await _guestUserRepository.CreateGuestUserAsync(guestUserDto);
        }
    }
}
