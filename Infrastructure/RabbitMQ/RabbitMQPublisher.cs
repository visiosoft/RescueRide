using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RescueRide.Infrastructure.RabbitMQ
{
    public class RabbitMQPublisher<T> : IRabbitMQPublisher<T>
    {
        private readonly ConnectionFactory _factory;

        public RabbitMQPublisher()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
        }

        public async Task PublishAsync(T message, string queueName)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            await Task.CompletedTask; // Simulating async for extensibility
        }
    }
}
