using Microsoft.AspNetCore.SignalR;
using RescueRide.Controllers;

namespace RescueRide.SignalR
{
    public class LocationHub : Hub
    {
        // Method to be called by clients to receive location updates
        public async Task SendLocationUpdate(DriverLocation location)
        {
            await Clients.All.SendAsync("ReceiveLocationUpdate", location);
        }

        // Optional: Handle client connection
        public override async Task OnConnectedAsync()
        {
            // You can perform actions like logging or tracking connection IDs here
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }
        public async Task SubscribeToDriver(string driverId)
        {
            var groupName = $"group_driver_{driverId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Console.WriteLine($"Client {Context.ConnectionId} subscribed to {groupName}");
        }

        // Method for clients to unsubscribe from a driver's updates
        public async Task UnsubscribeFromDriver(string driverId)
        {
            var groupName = $"group_driver_{driverId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            Console.WriteLine($"Client {Context.ConnectionId} unsubscribed from {groupName}");
        }

        // Handle disconnection to clean up group memberships
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Example: Cleanup logic can be added here
            Console.WriteLine($"Client {Context.ConnectionId} disconnected");
            await base.OnDisconnectedAsync(exception);
        }

    
    }
}
