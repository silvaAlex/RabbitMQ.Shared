using RabbitMQ.Client;

namespace RabbitMQ.Shared.MessageBus.Extensions
{
    public static class RabbitMQExtensions
    {
        public static IBasicProperties SetDurable(this IBasicProperties basicProperties, bool durable = true)
        {
            basicProperties.Persistent = durable;
            return basicProperties;
        }
    }
}
