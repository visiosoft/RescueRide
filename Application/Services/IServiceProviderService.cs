using RescueRide.Application.DTOs.DriversDTOs;
using RescueRide.Core.Models;

namespace RescueRide.Application.Services
{
    public interface IServiceProviderService
    {
        Task<IEnumerable<DriversDTO>> GetAllDriversAsync();
        Task<List<DriversDTO>> GetNearbyProvidersAsync(double latitude, double longitude, double radius);
        Task<Drivers> GetServiceProviderByIdAsync(int id);
        Task UpdateAvailabilityAsync(int serviceProviderId, bool isAvailable);
    }
}
