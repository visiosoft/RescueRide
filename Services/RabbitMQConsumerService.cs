using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

public class RabbitMQConsumerService
{
    private const string QueueName = "driver_location_queue";  // The name of the queue to consume from

    public void Start()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Read RabbitMQ settings
        var rabbitMqConfig = configuration.GetSection("RabbitMQ");
        var factory = new ConnectionFactory()
        {
            HostName = rabbitMqConfig["HostName"],
            Port = int.Parse(rabbitMqConfig["Port"]),
            UserName = rabbitMqConfig["UserName"],
            Password = rabbitMqConfig["Password"]
        };
            using (var connection = factory.CreateConnection())  // Create the connection
            using (var channel = connection.CreateModel())       // Create the channel
            {
                // Declare the queue
                channel.QueueDeclare(queue: QueueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                Console.WriteLine(" [*] Waiting for messages. Press [enter] to exit.");
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Received: {message}");
                };

                channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
                Console.ReadLine();
            }
    }
}
