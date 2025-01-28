using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RescueRide.Core.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Ensure MongoDB handles this as an ObjectId
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; } // Customer, ServiceProvider, Admin
        public string PasswordHash { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
    }

}
