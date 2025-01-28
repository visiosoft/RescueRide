using RescueRide.Application.DTOs.DriversDTOs;
using RescueRide.Core.Models;

namespace RescueRide.Infrastructure.Repositories
{
    public interface IServiceProviderRepository
    {
        Task<IEnumerable<Drivers>> GetAllDriversAsync();
        Task<List<DriversDTO>> GetNearbyDriversAsync(double latitude, double longitude, double radius);
        Task<Drivers> GetDriversByIdAsync(int id);
        Task UpdateAvailabilityAsync(int serviceProviderId, bool isAvailable);
    }

}
