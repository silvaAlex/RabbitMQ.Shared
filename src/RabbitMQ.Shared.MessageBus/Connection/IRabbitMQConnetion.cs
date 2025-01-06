using RabbitMQ.Client;

namespace RabbitMQ.Shared.MessageBus.Connection
{
    public interface IRabbitMQConnetion : IDisposable
    {
        IModel? CreateChannel();
    }
}