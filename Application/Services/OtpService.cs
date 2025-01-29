using FirebaseAdmin.Auth;

namespace RescueRide.Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly Dictionary<string, string> _otpStore = new();
        private readonly TimeSpan _otpExpiry = TimeSpan.FromMinutes(5);

        public async Task<string> SendOtp(string phoneNumber)
        {
            try
            {
                var sessionInfo = await FirebaseAuth.DefaultInstance.CreateSessionCookieAsync(phoneNumber, new SessionCookieOptions
                {
                    ExpiresIn = TimeSpan.FromMinutes(10)
                });

                Console.WriteLine($"OTP sent to {phoneNumber}");
                return sessionInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ValidateOtp(string idToken)
        {
            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                return decodedToken != null && decodedToken.Claims.ContainsKey("phone_number");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OTP validation failed: {ex.Message}");
                return false;
            }
        }
    }

}
