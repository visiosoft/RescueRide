using Microsoft.EntityFrameworkCore;
using RescueRide.Infrastructure.RabbitMQ;
using RescueRide.Application.DTOs.DriversDTOs;
using RescueRide.Infrastructure.Repositories;
using RescueRide.Core.Models;

namespace RescueRide.Application.Services
{
    public class ServiceProviderService : IServiceProviderService
    {
        private readonly IServiceProviderRepository _repository;
        private readonly IRabbitMQPublisher<IEnumerable<DriversDTO>> _rabbitMQPublisher;

        public ServiceProviderService(IServiceProviderRepository repository, IRabbitMQPublisher<IEnumerable<DriversDTO>> rabbitMQPublisher)
        {
            _repository = repository;
            _rabbitMQPublisher = rabbitMQPublisher;
        }
        public async Task<IEnumerable<DriversDTO>> GetAllDriversAsync()
        {
            var drivers = await _repository.GetAllDriversAsync();
            var driverDTOs = drivers.Select(d => new DriversDTO
            {
                Id = d.Id,
                Name = d.Name,
                Latitude = d.Latitude,
                Longitude = d.Longitude
            });
            // Publish to RabbitMQ
            await _rabbitMQPublisher.PublishAsync(driverDTOs, "driver_updates");
            return driverDTOs;
        }

        public async Task<List<DriversDTO>> GetNearbyProvidersAsync(double latitude, double longitude, double radius)
        {
            return await _repository.GetNearbyDriversAsync(latitude, longitude, radius);
        }

        public async Task<Drivers> GetServiceProviderByIdAsync(int id)
        {
            return await _repository.GetDriversByIdAsync(id);
        }

        public async Task UpdateAvailabilityAsync(int serviceProviderId, bool isAvailable)
        {
            await _repository.UpdateAvailabilityAsync(serviceProviderId, isAvailable);
        }
    }
}
