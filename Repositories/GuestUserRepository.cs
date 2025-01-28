using RescueRide.Data;
using RescueRide.DTOs.GuestDtos;
using RescueRide.Models;

namespace RescueRide.Repositories
{
    public class GuestUserRepository : IGuestUserRepository
    {
        private readonly AppDbContext _context;

        public GuestUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateGuestUserAsync(GuestUserDTO guestUserDto)
        {
            var guestUser = new GuestUser
            {
                PhoneNumber = guestUserDto.PhoneNumber,
                LicensePlate = guestUserDto.LicensePlate
            };

            _context.GuestUser.Add(guestUser);
            await _context.SaveChangesAsync();

            return guestUser.GuestUserId;
        }
    }

}
