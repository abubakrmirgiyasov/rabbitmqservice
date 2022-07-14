using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace ServiceAPI
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService()
        {
            try
            {
                _connection = RabbitMqConnection
                    .CreateConnection()
                    .CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare( "InboxQueue", true, false, false, null);

                _channel.QueueDeclare("OutboxQueue", true, false, false, null);

            }
            catch (BrokerUnreachableException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public string ConvertToJson(object obj)
        {
            var message = JsonSerializer.Serialize(obj);

            Publish(message);

            return message;
        }

        public string Publish(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: "",
                routingKey: "InboxQueue",
                basicProperties: null,
                body: buffer);

            return message;
        }

        public string Received(string queueName, Action<string> handler)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (s, e) =>
            {
                var content = Encoding.UTF8.GetString(e.Body.ToArray());

                handler(content);

                Console.WriteLine(content);

                _channel.BasicAck(e.DeliveryTag, false);
            };

            return _channel.BasicConsume(queueName, false, consumer);
        }
    }
}