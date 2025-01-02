using RabbitMQ.Client;

namespace RabbitMQ.Shared.MessageBus.Connection
{
    public class RabbitMQConnetion : IRabbitMQConnetion
    {
        private IConnection? _connection;
        private readonly string _connectionString;

        public RabbitMQConnetion(string connectionString)
        {
            _connectionString = connectionString;
            Connect();
        }

        public void Connect()
        {
            var factory = new ConnectionFactory()
            {
                Uri =new(_connectionString),
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
