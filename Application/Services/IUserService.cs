using RescueRide.Core.Models;

namespace RescueRide.Application.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(User user);
        Task<User> AuthenticateUserAsync(string email, string password);
        Task<User> GetUserByIdAsync(int userId);
    }
}
