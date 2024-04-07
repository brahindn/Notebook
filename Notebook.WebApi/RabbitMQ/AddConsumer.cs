using Notebook.Domain.Entities;
using Notebook.WebApi.Requests;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Text;
using Notebook.Application.Services.Contracts;
using System.Text.Json;
using Microsoft.Extensions.Hosting;

namespace Notebook.WebApi.RabbitMQ
{
    public class AddConsumer : BackgroundService
    {
        private readonly IServiceManager _serviceManager;
        public AddConsumer(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;

            //GetMessage();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "ForAdding",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var contact = JsonSerializer.Deserialize<ContactForCreateUpdateDTO>(message);

                await _serviceManager.ContactService.CreateContactAsync(contact.FirstName, contact.LastName, contact.PhoneNumber, contact.Email, contact.DateOfBirth);
            };
            channel.BasicConsume(queue: "ForAdding",
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        private void GetMessage()
        {
            
        }
    }
}
