using System.ComponentModel.DataAnnotations;

namespace RescueRide.Core.Models
{
    public class GuestUser
    {
        [Key]
        public int GuestUserId { get; set; }
        public string PhoneNumber { get; set; }
        public string LicensePlate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
