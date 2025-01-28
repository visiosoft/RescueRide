using RescueRide.Core.Models;

namespace RescueRide.Application.Services
{
    public interface IVehicleService
    {
        Task AddVehicleAsync(Vehicle vehicle);
        Task<List<Vehicle>> GetVehiclesByUserIdAsync(int userId);
        Task UpdateVehicleAsync(Vehicle vehicle);
        Task DeleteVehicleAsync(int vehicleId);
    }

}
