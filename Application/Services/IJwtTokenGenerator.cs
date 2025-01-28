using RescueRide.Core.Models;

namespace RescueRide.Application.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }

}
