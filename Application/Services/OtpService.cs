namespace RescueRide.Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly Dictionary<string, string> _otpStore = new();
        private readonly TimeSpan _otpExpiry = TimeSpan.FromMinutes(5);

        public void SendOtp(string username)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            _otpStore[username] = otp;

            // Simulate sending OTP (replace with Twilio, Firebase, etc.)
            Console.WriteLine($"OTP for {username}: {otp}");
        }

        public bool ValidateOtp(string username, string otp)
        {
            return _otpStore.TryGetValue(username, out var storedOtp) && storedOtp == otp;
        }
    }

}
