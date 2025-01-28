using RescueRide.Application.DTOs.Authentication;

namespace RescueRide.Application.Services
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> Authenticate(string username, string password);
        void GenerateOtp(string username);
        bool ValidateOtp(string username, string otp);
    }
}
