using RescueRide.DTOs.ServiceProviderDtos;
using RescueRide.Models;

namespace RescueRide.Repositories
{
    public interface IServiceProviderRepository
    {
        Task<IEnumerable<Drivers>> GetAllDriversAsync();
        Task<List<DriversDTO>> GetNearbyDriversAsync(double latitude, double longitude, double radius);
        Task<Drivers> GetDriversByIdAsync(int id);
        Task UpdateAvailabilityAsync(int serviceProviderId, bool isAvailable);
    }

}
