namespace RescueRide.Application.Services
{
    public interface IOtpService
    {
        Task<string> SendOtp(string phoneNumber);
        Task<bool> ValidateOtp(string idToken);
    }

}
