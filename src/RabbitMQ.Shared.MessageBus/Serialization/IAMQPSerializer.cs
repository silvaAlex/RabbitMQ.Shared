using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Shared.MessageBus.Serialization
{
    public interface IAMQPSerializer
    {
        TMessage? Deserialize<TMessage>(BasicDeliverEventArgs eventArgs);
        object? Deserialize(BasicDeliverEventArgs eventArgs, Type type);
        byte[]? Serialize<T>(IBasicProperties basicProperties, T message);
        byte[]? Serialize(IBasicProperties basicProperties, object message);
    }
}
