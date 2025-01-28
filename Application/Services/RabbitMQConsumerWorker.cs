public class RabbitMQConsumerWorker : BackgroundService
{
    private readonly RabbitMQConsumerService _consumerService;

    public RabbitMQConsumerWorker(RabbitMQConsumerService consumerService)
    {
        _consumerService = consumerService;
    }

    // This method runs on a separate thread as part of the background service
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Start the RabbitMQ Consumer service asynchronously
        Task.Run(() => _consumerService.Start());

        return Task.CompletedTask;
    }
}
