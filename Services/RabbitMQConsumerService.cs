using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RescueRide.Controllers;
using RescueRide.Data;

namespace RescueRide.Services
{
    public class RabbitMQConsumerService
    {
        
            private const string QueueName = "driver_location_queue";  // The name of the queue to consume from
            private const string HostName = "localhost";               // RabbitMQ server hostname

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

        private void  ProcessMessage(string message)
        {
            // Deserialize the message to DriverLocation (or you can use a library like Newtonsoft.Json to do this)
            // Here, assume the message is a JSON string that can be directly deserialized into DriverLocation
            var location = Newtonsoft.Json.JsonConvert.DeserializeObject<DriverLocation>(message);

            // Save the location to the database
            if (location != null)
            {
                DriverLocationStore.DriverLocations[location.DriverId] = location;

                // _context.DriverLocations.Add(location);
                // await _context.SaveChangesAsync();
                Console.WriteLine($"Saved location for driver {location.DriverId} at {location.Timestamp}");
            }
            else
            {
                Console.WriteLine("Failed to deserialize the message into a DriverLocation.");
            }
        }
    }
}
