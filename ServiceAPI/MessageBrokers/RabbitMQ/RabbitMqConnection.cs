using RabbitMQ.Client;

namespace ServiceAPI
{
    public class RabbitMqConnection
    {
        private readonly IConfiguration _configuration;

        public RabbitMqConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static ConnectionFactory CreateConnection()
        {
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            var factory = new ConnectionFactory();
            config.GetSection("RabbitMq").Bind(factory);

            return factory;
        }

        public int GetMessageLife()
        {
            return _configuration.GetValue<int>("RabbitMq:MessageLifeTimeInMinutes");
        }
    }
}
