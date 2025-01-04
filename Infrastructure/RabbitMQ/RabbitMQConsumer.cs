using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RescueRide.Infrastructure.RabbitMQ
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly ConnectionFactory _factory;

        public RabbitMQConsumer()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
        }

        public async Task ConsumeAsync<T>(string queueName, Func<T, Task> onMessageReceived)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var jsonMessage = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<T>(jsonMessage);

                if (message != null)
                {
                    await onMessageReceived(message);
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            await Task.CompletedTask; // Keeps the consumer running
        }
    }
}
