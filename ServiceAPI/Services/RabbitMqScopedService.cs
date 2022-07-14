namespace ServiceAPI
{
    public interface IRabbitMqScopedService
    {
        Task DoWork(CancellationToken cancellationToken);
    }

    public class RabbitMqScopedService : IRabbitMqScopedService
    {
        private readonly ILogger<RabbitMqScopedService> _logger;

        public RabbitMqScopedService(ILogger<RabbitMqScopedService> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Delay(100 * 20, cancellationToken);
        }
    }
}
