namespace ServiceAPI
{
    public interface IMessageBusWrapper
    {
        string Received(Guid id);

        Guid Send(Service service);

        void ExceptionHandler(string message, string error);
    }
}
