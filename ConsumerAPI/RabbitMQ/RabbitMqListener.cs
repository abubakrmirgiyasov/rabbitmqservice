using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace ConsumerAPI
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqListener()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(
                    queue: "InboxQueue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            }
            catch (BrokerUnreachableException)
            {
                Console.WriteLine("Сервер RabbitMq недоступен.");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            try
            {
                var consumer = new EventingBasicConsumer(_channel);

                if (_channel != null)
                {
                    consumer.Received += (c, e) =>
                    {
                        var content = Encoding.UTF8.GetString(e.Body.ToArray());

                        SendToOutboxQueue(content);

                        _channel.BasicAck(e.DeliveryTag, false);
                    };

                    _channel.BasicConsume("InboxQueue", false, consumer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Task.CompletedTask;
        }

        public void SendToOutboxQueue(object obj)
        {
            var message = JsonSerializer.Serialize(obj);
            SendToOutboxQueue(message);
        }

        public void SendToOutboxQueue(string message)
        {
            try
            {
                if (_channel != null)
                {
                    var body = Encoding.UTF8.GetBytes(message);

                    _channel.BasicPublish(
                        exchange: "",
                        routingKey: "OutboxQueue",
                        basicProperties: null,
                        body: body);

                    Console.WriteLine($"Message was sent to OutboxQueue. {message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
