using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Notebook.WebApi.RabbitMQ
{
    public class MessageProducer
    {
        private RabbitMqSettings _rabbitMqSettings;
        public MessageProducer(RabbitMqSettings rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings;
        }
        public void SendMessage<T>(T message, string routingKey)
        {
            var factory = new ConnectionFactory { HostName = _rabbitMqSettings.StringHostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "direct_actions", type: ExchangeType.Direct);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "direct_actions",
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
