using RabbitMQ.Client;

namespace Notebook.WebApi.RabbitMQ.Connection
{
    public class RabbitMQConnection : IRabbitMQConnection, IDisposable
    {
        private IConnection _connection;
        public IConnection Connection => _connection;

        public RabbitMQConnection()
        {
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
