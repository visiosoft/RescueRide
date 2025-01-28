namespace RescueRide.Application.DTOs.Authentication
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string? ErrorMessage { get; set; }
    }

}
