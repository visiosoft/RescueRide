namespace RescueRide.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public int CustomerId { get; set; }
        public int ServiceProviderId { get; set; }
        public int GuestUserId { get; set; }
        public int VehicleId { get; set; }
        public string ServiceType { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public string Status { get; set; } // Requested, Accepted, InProgress, Completed
        public decimal TotalCost { get; set; }
    }

}
