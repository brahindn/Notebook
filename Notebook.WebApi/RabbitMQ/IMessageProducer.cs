
namespace Notebook.WebApi.RabbitMQ
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}
