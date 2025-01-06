using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Shared.MessageBus.Settings;

namespace RabbitMQ.Shared.MessageBus.Connection
{
    public class RabbitMQConnetion : IRabbitMQConnetion
    {
        private IConnection? _connection;
        private readonly RabbitMQSetting _rabbitMqSetting;

        public RabbitMQConnetion(IOptions<RabbitMQSetting> setting)
        {
            _rabbitMqSetting = setting.Value;
            Connect();
        }

        private void Connect()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };

            _connection = factory.CreateConnection();
        }

        public IModel? CreateChannel()
        {
            if (_connection == null || !_connection.IsOpen)
                Connect();

            return _connection?.CreateModel();
        }

        public void Dispose()
        {
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
