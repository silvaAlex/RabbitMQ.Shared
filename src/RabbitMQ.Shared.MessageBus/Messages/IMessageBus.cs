namespace RabbitMQ.Shared.MessageBus.Messages
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T  message, string exchangeName) where T : class;
        void Subscribe<T>(string subscriptionId, string exchangeName) where T : class;
    }
}
