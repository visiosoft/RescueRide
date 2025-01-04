using RescueRide.Models;

namespace RescueRide.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(User user);
        Task<User> AuthenticateUserAsync(string email, string password);
        Task<User> GetUserByIdAsync(int userId);
    }
}
