using RescueRide.Core.Models;

namespace RescueRide.Application.Services
{
    public interface IVehicleService
    {
        Task AddVehicleAsync(Vehicle vehicle);
        Task<List<Vehicle>> GetVehiclesByUserIdAsync(string userId);
        Task UpdateVehicleAsync(Vehicle vehicle);
        Task DeleteVehicleAsync(int vehicleId);
    }

}
