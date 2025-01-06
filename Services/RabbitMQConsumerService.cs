using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using RescueRide.SignalR;
using System;
using RescueRide.Controllers;
using RescueRide.Data;

namespace RescueRide.Services
{
    public class RabbitMQConsumerService
    {
        private const string QueueName = "driver_location_queue";  // The name of the queue to consume from
        private const string HostName = "https://14cc-87-201-97-245.ngrok-free.app/";
        private readonly IServiceProvider _serviceProvider;  // To resolve IHubContext<LocationHub> dynamically

        // Inject IServiceProvider into constructor
        public RabbitMQConsumerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Start()
        {
            // Create a connection factory
            var factory = new ConnectionFactory() { HostName = HostName };

            // Create a connection to RabbitMQ
            using (var connection = factory.CreateConnection())
            // Create a channel to communicate with RabbitMQ
            using (var channel = connection.CreateModel())
            {
                // Declare the queue (it must exist)
                channel.QueueDeclare(queue: QueueName,
                                     durable: true,      // The queue will survive server restarts
                                     exclusive: false,   // The queue will not be exclusive to a connection
                                     autoDelete: false,  // The queue won't be automatically deleted when unused
                                     arguments: null);   // No custom arguments

                // Create a consumer
                var consumer = new EventingBasicConsumer(channel);

                // Define what happens when a message is received
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Received: {message}");

                    // Process the message (You can add your logic here)
                    ProcessMessage(message);
                };

                // Start consuming messages from the queue
                channel.BasicConsume(queue: QueueName,
                                     autoAck: true,  // Acknowledge the message automatically
                                     consumer: consumer);

                Console.WriteLine(" [*] Waiting for messages. Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        public static Dictionary<string, DriverLocation> DriverLocations = new Dictionary<string, DriverLocation>();

        private void ProcessMessage(string message)
        {
            // Deserialize the message to DriverLocation (or you can use a library like Newtonsoft.Json to do this)
            var location = Newtonsoft.Json.JsonConvert.DeserializeObject<DriverLocation>(message);

            // Resolve the IHubContext using IServiceProvider
            using (var scope = _serviceProvider.CreateScope())
            {
                // Get IHubContext<LocationHub> from the service provider
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<LocationHub>>();

                // Broadcast the location update to all clients
                hubContext.Clients.All.SendAsync("ReceiveLocationUpdate", location);
            }

            // Save the location to the dictionary (or database if needed)
            if (location != null)
            {
                DriverLocationStore.DriverLocations[location.DriverId] = location;
                Console.WriteLine($"Saved location for driver {location.DriverId} at {location.Timestamp}");
            }
            else
            {
                Console.WriteLine("Failed to deserialize the message into a DriverLocation.");
            }
        }
    }
}
