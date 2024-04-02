using RabbitMQ.Client;

namespace Notebook.WebApi.RabbitMQ.Connection
{
    public interface IRabbitMQConnection
    {
        IConnection Connection { get; }
    }
}
