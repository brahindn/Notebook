﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Notebook.Domain.Requests;
using Notebook.Application.Services.Contracts;

namespace Notebook.WebApi.RabbitMQ
{
    public class UpdateConsumer : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;
        private RabbitMqSettings _rabbitMqSettings;

        public UpdateConsumer(IServiceScopeFactory serviceScopeFactory, RabbitMqSettings rabbitMqSettings)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _rabbitMqSettings = rabbitMqSettings;
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "direct_actions", type: ExchangeType.Direct);

            _queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queue: _queueName,
                               exchange: "direct_actions",
                               routingKey: "UpdateKey");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var contact = JsonSerializer.Deserialize<ContactForUpdateDTO>(message);
                var address = JsonSerializer.Deserialize<AddressForUpdateDTO>(message);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var serviceManager = scope.ServiceProvider.GetRequiredService<IServiceManager>();

                    if(contact.FirstName != null)
                    {
                        await serviceManager.ContactService.UpdateContactAsync(contact.Id, contact.FirstName, contact.LastName, contact.PhoneNumber, contact.Email, contact.DateOfBirth);
                    }
                    else
                    {
                        await serviceManager.AddressService.UpdateAddressAsync(address.Id, address.AddressType, address.Country, address.Region, address.City, address.Street, address.BuildingNumber);
                    }
                }
            };

            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _connection.Close();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
