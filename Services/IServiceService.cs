using RescueRide.Models;

namespace RescueRide.Services
{
    public interface IServiceService
    {
        Task<int> RequestServiceAsync(Service service);
        Task<Service> GetServiceByIdAsync(int serviceId);
        Task<List<Service>> GetServicesByUserIdAsync(int userId);
        Task UpdateServiceStatusAsync(int serviceId, string status);
        Task CancelServiceAsync(int serviceId);
    }

}
