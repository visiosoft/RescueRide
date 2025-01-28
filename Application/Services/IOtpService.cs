namespace RescueRide.Application.Services
{
    public interface IOtpService
    {
        void SendOtp(string username);
        bool ValidateOtp(string username, string otp);
    }

}
