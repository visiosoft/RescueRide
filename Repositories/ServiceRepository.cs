using RescueRide.Data;
using RescueRide.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace RescueRide.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddServiceAsync(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return service.ServiceId;
        }

        public async Task<Service> GetServiceByIdAsync(int serviceId)
        {
            return await _context.Services.FindAsync(serviceId);
        }

        public async Task<List<Service>> GetServicesByUserIdAsync(int userId)
        {
            return await _context.Services.Where(s => s.CustomerId == userId).ToListAsync();
        }

        public async Task UpdateServiceStatusAsync(int serviceId, string status)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service != null)
            {
                service.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CancelServiceAsync(int serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
        }
    }

}
