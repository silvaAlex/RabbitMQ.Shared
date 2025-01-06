
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Shared.MessageBus.Connection;
using RabbitMQ.Shared.MessageBus.Extensions;
using RabbitMQ.Shared.MessageBus.Serialization;

namespace RabbitMQ.Shared.MessageBus.Messages
{
    public class MessageBus(IRabbitMQConnetion connetion, IAMQPSerializer serializer) : IMessageBus
    {
        private readonly IRabbitMQConnetion _connetion = connetion;
        private readonly IAMQPSerializer _serializer = serializer;

        public async Task PublishAsync<T>(T message, string exchangeName) where T : class
        {
            var channel = _connetion.CreateChannel();
            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, durable: true);

            var basicProperties = channel?.CreateBasicProperties().EnsureHeaders().SetDurable(true); ;

            ArgumentNullException.ThrowIfNull(basicProperties, nameof(basicProperties));

            var body = _serializer.Serialize(basicProperties, message);
            channel.BasicPublish(
                exchange: exchangeName,
                routingKey: string.Empty, // Usamos Fanout, então não há chave de roteamento
                basicProperties: null,
                body: body
            );

            await Task.CompletedTask;

        }

        public void Subscribe<T>(string subscriptionId, string exchangeName) where T : class
        {
            var channel = _connetion.CreateChannel();

            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, durable: true);
            var queueName = $"{subscriptionId}.{typeof(T).Name}";
            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queueName, exchangeName, routingKey: string.Empty);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var message = _serializer.Deserialize<T>(eventArgs);

                Console.WriteLine($"Mensagem recebida: {message}");

                channel?.BasicAck(eventArgs.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }
    }
}
