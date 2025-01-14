﻿using RescueRide.Models;

namespace RescueRide.Repositories
{
    public interface IServiceRepository
    {
        Task<int> AddServiceAsync(Service service);
        Task<Service> GetServiceByIdAsync(int serviceId);
        Task<List<Service>> GetServicesByUserIdAsync(int userId);
        Task UpdateServiceStatusAsync(int serviceId, string status);
        Task CancelServiceAsync(int serviceId);
    }
}
