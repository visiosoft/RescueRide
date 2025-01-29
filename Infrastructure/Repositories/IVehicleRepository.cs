using RescueRide.Core.Models;

namespace RescueRide.Infrastructure.Repositories
{
    public interface IVehicleRepository
    {
        Task AddVehicleAsync(Vehicle vehicle);
        Task<List<Vehicle>> GetVehiclesByUserIdAsync(string userId);
        Task UpdateVehicleAsync(Vehicle vehicle);
        Task DeleteVehicleAsync(int vehicleId);
    }
}
