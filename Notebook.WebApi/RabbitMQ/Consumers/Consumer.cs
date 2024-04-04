using Notebook.Application.Services.Contracts;
using Notebook.WebApi.Requests;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Notebook.WebApi.RabbitMQ.Consumers
{
    public class Consumer
    {
        private readonly IServiceManager _serviceManager;

        public Consumer(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
            Main();
        }

        private void Main()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();
            channel.QueueDeclare("ForAdding", exclusive: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var contact = JsonSerializer.Deserialize<ContactForCreateUpdateDTO>(message);

                AddNewContact(contact);

            };

            channel.BasicConsume(queue: "ForAdding", autoAck: true, consumer: consumer);
        }

        private async Task AddNewContact(ContactForCreateUpdateDTO contact)
        {
            await _serviceManager.ContactService.CreateContactAsync(contact.FirstName, contact.LastName, contact.PhoneNumber, contact.Email, contact.DateOfBirth);
        }
    }
}
