namespace RescueRide.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; } // Customer, ServiceProvider, Admin
        public string PasswordHash { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
    }

}
