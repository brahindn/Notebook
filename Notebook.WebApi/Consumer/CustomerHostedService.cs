using Microsoft.Extensions.Hosting;
using Notebook.WebApi.Requests;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Notebook.Application.Services.Contracts;
using Newtonsoft.Json;

namespace Notebook.WebApi.Customers
{
    public class CustomerHostedService : BackgroundService
    {
        private readonly IServiceManager _serviceManager;

        public CustomerHostedService(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(
                            queue: "ForAdding",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                        ContactForCreateUpdateDTO contact;

                        consumer.Received += async (sender, e) =>
                        {
                            try
                            {
                                var body = e.Body;
                                var message = Encoding.UTF8.GetString(body.ToArray());

                                contact = JsonConvert.DeserializeObject<ContactForCreateUpdateDTO>(message);

                                await _serviceManager.ContactService.CreateContactAsync(contact.FirstName, contact.LastName, contact.PhoneNumber, contact.Email, contact.DateOfBirth);
                            }
                            catch (Exception ex)
                            {
                                throw ex.InnerException;
                            }
                        };

                        channel.BasicConsume(
                            queue: "ForAdding",
                            autoAck: true,
                            consumer: consumer);
                    }
                }
            }
        }
    }
}
