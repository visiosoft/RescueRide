using RescueRide.DTOs.GuestDtos;
using RescueRide.Repositories;

namespace RescueRide.Services
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
