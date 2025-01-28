using RescueRide.Application.DTOs.GuestDTOs;
using RescueRide.Core.Models;
using RescueRide.Infrastructure.Data;

namespace RescueRide.Infrastructure.Repositories
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
