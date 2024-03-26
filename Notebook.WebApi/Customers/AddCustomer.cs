using Notebook.Domain.Entities;
using Notebook.WebApi.Requests;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Notebook.Application.Services.Contracts;

namespace Notebook.WebApi.Customers
{
    public class AddCustomer
    {
        private readonly IServiceManager _serviceManager;

        public AddCustomer(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public void Add()
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
