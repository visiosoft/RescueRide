using RescueRide.Models;
using RescueRide.Repositories;

namespace RescueRide.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
        }

        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }
    }
}
