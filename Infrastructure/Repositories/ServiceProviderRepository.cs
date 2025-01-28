using Microsoft.EntityFrameworkCore;
using RescueRide.Application.DTOs.DriversDTOs;
using RescueRide.Core.Models;
using RescueRide.Infrastructure.Data;

namespace RescueRide.Infrastructure.Repositories
{
    public class ServiceProviderRepository : IServiceProviderRepository
    {
        private readonly AppDbContext _context;

        public ServiceProviderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Drivers>> GetAllDriversAsync()
        {
            return await _context.Drivers.Where(d => d.IsAvailable).ToListAsync();
        }
        public async Task<List<DriversDTO>> GetNearbyDriversAsync(double latitude, double longitude, double radius)
        {
            // Simple approximation using latitude/longitude within a bounding box
            return await _context.Drivers
                .Where(sp => sp.IsAvailable)
                .Select(sp => new DriversDTO
                {
                    Id = sp.Id,
                    Name = sp.Name,
                    Latitude = sp.Latitude,
                    Longitude = sp.Longitude,
                    Distance = CalculateDistance(latitude, longitude, sp.Latitude, sp.Longitude)
                })
                .Where(sp => sp.Distance <= radius)
                .OrderBy(sp => sp.Distance)
                .ToListAsync();
        }

        public async Task<Drivers> GetDriversByIdAsync(int id)
        {
            return await _context.Drivers.FindAsync(id);
        }

        public async Task UpdateAvailabilityAsync(int serviceProviderId, bool isAvailable)
        {
            var provider = await _context.Drivers.FindAsync(serviceProviderId);
            if (provider != null)
            {
                provider.IsAvailable = isAvailable;
                await _context.SaveChangesAsync();
            }
        }

        // Simple distance calculation (replace with geospatial functions for accuracy)
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var earthRadiusKm = 6371;
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return earthRadiusKm * c;
        }

        private double DegreesToRadians(double degrees) => degrees * Math.PI / 180;
    }

}
