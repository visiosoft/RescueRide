using RescueRide.Models;
using RescueRide.Repositories;

namespace RescueRide.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<int> RequestServiceAsync(Service service)
        {
            return await _serviceRepository.AddServiceAsync(service);
        }

        public async Task<Service> GetServiceByIdAsync(int serviceId)
        {
            return await _serviceRepository.GetServiceByIdAsync(serviceId);
        }

        public async Task<List<Service>> GetServicesByUserIdAsync(int userId)
        {
            return await _serviceRepository.GetServicesByUserIdAsync(userId);
        }

        public async Task UpdateServiceStatusAsync(int serviceId, string status)
        {
            await _serviceRepository.UpdateServiceStatusAsync(serviceId, status);
        }

        public async Task CancelServiceAsync(int serviceId)
        {
            await _serviceRepository.CancelServiceAsync(serviceId);
        }
    }
}
