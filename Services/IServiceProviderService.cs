using RescueRide.DTOs.ServiceProviderDtos;

namespace RescueRide.Services
{
    public interface IServiceProviderService
    {
        Task<IEnumerable<DriversDTO>> GetAllDriversAsync();
        Task<List<DriversDTO>> GetNearbyProvidersAsync(double latitude, double longitude, double radius);
        Task<Models.Drivers> GetServiceProviderByIdAsync(int id);
        Task UpdateAvailabilityAsync(int serviceProviderId, bool isAvailable);
    }
}
