
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Shared.MessageBus.Connection;
using RabbitMQ.Shared.MessageBus.Helper;
using System.Text;

namespace RabbitMQ.Shared.MessageBus.Messages
{
    public class MessageBus(IRabbitMQConnetion connetion) : IMessageBus
    {
        private readonly IRabbitMQConnetion _connetion = connetion;

        public async Task PublishAsync<T>(T message, string exchangeName) where T : class
        {
            var channel = _connetion.CreateChannel();
            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, durable: true);
            var body = JsonSerialization.SerializeMessageInBytes(message);
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
                var body = eventArgs.Body.ToArray();
                var message = JsonSerialization.DeserializeMessage<T>(Encoding.UTF8.GetString(body));

                Console.WriteLine($"Mensagem recebida: {message}");

                channel?.BasicAck(eventArgs.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }
    }
}
