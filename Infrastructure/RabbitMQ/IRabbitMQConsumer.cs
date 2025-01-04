namespace RescueRide.Infrastructure.RabbitMQ
{
    public interface IRabbitMQConsumer
    {
        Task ConsumeAsync<T>(string queueName, Func<T, Task> onMessageReceived);
    }
}
