using System.Text.Json;

namespace ServiceAPI
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IRabbitMqService _send;

        public RabbitMqListener(IRabbitMqService send)
        {
            _send = send;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _send.Received("OutboxQueue", UpdateConvertedMessage);

            return Task.CompletedTask;
        }

        public void UpdateConvertedMessage(string message)
        {
            using var db = new ApplicationDbContext();

            var service = JsonSerializer.Deserialize<Service>(message);

            service.Message = $"Get message from outboxqueue: {service.Message}";
            service.Status = "Processed";
            service.CreatedDate = DateTime.Now;

            db.Messages.Update(service);

            db.SaveChanges();

            Console.WriteLine($"Updated. \nID: {service.Id} \t{service.Message}");
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}