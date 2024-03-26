using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Notebook.Application.Services.Contracts;
using Notebook.WebApi.Customers;
using Notebook.WebApi.Requests;
using Notebook.WebApi.Responses;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Notebook.WebApi.Controllers
{
    [Route("api/contacts")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly Serilog.ILogger _logger;

        public ContactController(IServiceManager serviceManager, Serilog.ILogger logger)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddContact([FromBody] ContactForCreateUpdateDTO contact)
        {
            if (contact == null)
            {
                return BadRequest("ContactForCreateUpdateDTO object is null");
            }

            try
            {
                SendTaskIntoAddQueue(contact);

                //_logger.Information($"New contact {contact.FirstName} {contact.LastName} has been added successfully");

                //ReaderMessages();

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

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"CreateContact error: {ex.Message}");
            }
        }

        [HttpPut("{update}")]
        public async Task<IActionResult> UpdateContact(Guid contactId, [FromBody] ContactForCreateUpdateDTO contact)
        {
            if (contact == null)
            {
                return BadRequest("ContactForCreateUpdateDTO object is null");
            }

            string routingKey = "UpdateContactKey";

            try
            {
                var existContact = await _serviceManager.ContactService.GetContactAsync(contactId);

                if (existContact == null)
                {
                    return NotFound();
                }

                SendTaskIntoAddQueue(contact);

                /*await _serviceManager.ContactService.UpdateContactAsync(contactId, contact.FirstName, contact.LastName, contact.PhoneNumber, contact.Email, contact.DateOfBirth);*/

                _logger.Information($"Contact {contactId} has been updated successfully!");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"UpdateContact error: {ex.Message}");
            }
        }

        [HttpDelete("{delete}")]
        public async Task<IActionResult> DeleteContact([FromQuery] Guid contactId)
        {
            var existContact = await _serviceManager.ContactService.GetContactAsync(contactId);

            if (existContact == null)
            {
                return NotFound();
            }

            try
            {
                await _serviceManager.ContactService.DeleteContactAsync(existContact);

                _logger.Information($"Contact deleted: {existContact.Id}");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"DeleteContact error: {ex.Message}");
            }
        }

        [HttpGet("{getById}")]
        public async Task<IActionResult> GetContactById(Guid contactId)
        {
            try
            {
                var contact = await _serviceManager.ContactService.GetContactAsync(contactId);

                if (contact == null)
                {
                    return NotFound();
                }

                var contactDTO = contact;

                _logger.Information($"Contact {contactDTO.Id} has been got");

                return Ok(contactDTO);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"GetContact error: {ex.Message}");
            }
        }

        [HttpGet("{getAll}")]
        public IActionResult GetAllContacts()
        {
            try
            {
                var allContacts = _serviceManager.ContactService.GetAllContacts();

                var contactDTO = allContacts.Select(c => new ContactResponseDTO
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email,
                    DateOfBirth = (DateTime)c.DateOfBirth
                }).ToList();

                _logger.Information("All contacts have been got");

                return Ok(contactDTO);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"GetContacts error: {ex.Message}");
            }
        }

        private static void SendTaskIntoAddQueue(ContactForCreateUpdateDTO contact)
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

                    string message = JsonConvert.SerializeObject(contact);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(
                    exchange: "",
                    routingKey: "ForAdding",
                    basicProperties: null,
                    body: body);
                }
            }
        }

        /*private void ReaderMessages()
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

                            await WriteInDB(contact);
                        }
                        catch(Exception ex)
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
        }*/

        /*private async Task WriteInDB(ContactForCreateUpdateDTO contact)
        {
            await _serviceManager.ContactService.CreateContactAsync(contact.FirstName, contact.LastName, contact.PhoneNumber, contact.Email, contact.DateOfBirth);
        }*/
    }
}
