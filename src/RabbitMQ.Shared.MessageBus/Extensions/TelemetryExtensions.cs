using RabbitMQ.Client;

namespace RabbitMQ.Shared.MessageBus.Extensions
{
    public static class TelemetryExtensions
    {
        public static IBasicProperties EnsureHeaders(this IBasicProperties basicProperties)
        {
            ArgumentNullException.ThrowIfNull(basicProperties);
            basicProperties.Headers ??= new Dictionary<string, object>();
            return basicProperties;
        }
    }
}
