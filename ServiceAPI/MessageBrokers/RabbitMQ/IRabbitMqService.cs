namespace ServiceAPI
{
    public interface IRabbitMqService
    {
        string ConvertToJson(object obj);

        string Publish(string message);

        string Received(string queueName, Action<string> handler);
    }
}
