using Notebook.WebApi.RabbitMQ.Connection;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Notebook.WebApi.RabbitMQ
{
    public class MessageProducer : IMessageProducer
    {
        private IRabbitMQConnection _connection;

        public MessageProducer(IRabbitMQConnection connection)
        {
            _connection = connection;
        }
        public void SendMessage<T>(T message)
        {
            using var channel = _connection.Connection.CreateModel();

            channel.QueueDeclare("ForAdding", exclusive: false);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "ForAdding", body: body);
        }
    }
}
