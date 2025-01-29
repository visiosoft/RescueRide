﻿namespace RescueRide.Core.Models
{
    public class LocationHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
