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

            // Optional: Handle client disconnection
            public override async Task OnDisconnectedAsync(Exception? exception)
            {
                Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
                await base.OnDisconnectedAsync(exception);
        }
    }
}
