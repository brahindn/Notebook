using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Notebook.WebApi.RabbitMQ
{
    public class MessageProducer
    { 
        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "ForAdding",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "ForAdding",
                                 basicProperties: null,
                                 body: body);
        }
    }
}
