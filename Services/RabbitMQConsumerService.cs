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
                                                                   // private const string HostName = "localhost";
        private readonly IServiceProvider _serviceProvider;  // To resolve IHubContext<LocationHub> dynamically
        private const string HostName = "0.tcp.ngrok.io";  // Ngrok public hostname
        private const int Port = 19881;  // Ngrok-provided port
        private readonly MongoDbService _mongoDbService;

        // Inject IServiceProvider into constructor
        public RabbitMQConsumerService(IServiceProvider serviceProvider, MongoDbService mongoDbService)
        {
            _serviceProvider = serviceProvider;
            _mongoDbService = mongoDbService;

        }

        public void Start()
        {
            // Create a connection factory with Ngrok-provided URL
            var factory = new ConnectionFactory()
            {
                HostName = "0.tcp.in.ngrok.io",  // Ngrok public hostname
                Port = 19881,                    // Ngrok forwarded port
                UserName = "guest",              // Default RabbitMQ username
                Password = "guest"               // Default RabbitMQ password
            };

            // Create a connection to RabbitMQ
            using (var connection = factory.CreateConnection())
            // Create a channel to communicate with RabbitMQ
            using (var channel = connection.CreateModel())
            {
                // Declare the queue
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

                    // Process the message
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

        private async void ProcessMessage(string message)
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

                await _mongoDbService.InsertDocumentAsync(location, "DriverLocations"); // Make sure collection name matches

            }
            else
            {
                Console.WriteLine("Failed to deserialize the message into a DriverLocation.");
            }
        }
    }


}
