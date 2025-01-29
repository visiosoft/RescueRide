using RescueRide.Core.Models;

namespace RescueRide.Infrastructure.Repositories
{
    public interface IServiceRepository
    {
        Task<int> AddServiceAsync(Service service);
        Task<Service> GetServiceByIdAsync(int serviceId);
        Task<List<Service>> GetServicesByUserIdAsync(string userId);
        Task UpdateServiceStatusAsync(int serviceId, string status);
        Task CancelServiceAsync(int serviceId);
    }
}
