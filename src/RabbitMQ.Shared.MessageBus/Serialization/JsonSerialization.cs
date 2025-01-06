using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;


namespace RabbitMQ.Shared.MessageBus.Serialization
{
    public class JsonSerialization(JsonSerializerOptions options) : IAMQPSerializer
    {
        private readonly JsonSerializerOptions options = options ?? new();

        public TMessage? Deserialize<TMessage>(BasicDeliverEventArgs eventArgs)
        {
            ArgumentNullException.ThrowIfNull(eventArgs);

            var bytes = eventArgs.Body.ToArray();
            if (bytes.Length > 0)
            {
                var message = Encoding.UTF8.GetString(bytes);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    return JsonSerializer.Deserialize<TMessage>(message, options);
                }
            }
            return default;
        }

        public object? Deserialize(BasicDeliverEventArgs eventArgs, Type type)
        {
            ArgumentNullException.ThrowIfNull(eventArgs);
            ArgumentNullException.ThrowIfNull(type);

            var bytes = eventArgs.Body.ToArray();
            if (bytes.Length > 0)
            {
                var message = Encoding.UTF8.GetString(bytes);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    return JsonSerializer.Deserialize(message, type, options);
                }
            }
            return default;
        }

        public byte[] Serialize<T>(IBasicProperties basicProperties, T message)
        {
            ArgumentNullException.ThrowIfNull(basicProperties);

            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, options));
        }

        public byte[] Serialize(IBasicProperties basicProperties, object message)
        {
            ArgumentNullException.ThrowIfNull(basicProperties);

            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, options));
        }
    }
}
