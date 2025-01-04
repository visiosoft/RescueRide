namespace RescueRide.Infrastructure.RabbitMQ
{
    public interface IRabbitMQPublisher<T>
    {
        Task PublishAsync(T message, string queueName);
    }
}
