using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
using Notebook.Shared.RequestFeatures;
using Notebook.WebApi.RabbitMQ;
using Notebook.Domain.Requests;

namespace Notebook.WebApi.Controllers
{
    [Route("api/contacts")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly Serilog.ILogger _logger;
        private readonly MessageProducer _messageProducer;

        public ContactController(IServiceManager serviceManager, Serilog.ILogger logger, MessageProducer messageProducer)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            _messageProducer = messageProducer;
        }

        [HttpPost("add")]
        public Task<IActionResult> AddContact([FromBody] ContactForCreateDTO contact)
        {

            var routingKey = "AddKey";

            _messageProducer.SendMessage(contact, routingKey);

            _logger.Information($"New contact {contact.FirstName} {contact.LastName} has been added successfully");

            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpPut("{update}")]
        public Task<IActionResult> UpdateContact([FromBody] ContactForUpdateDTO contact)
        {
            var routingKey = "UpdateKey";

            _messageProducer.SendMessage(contact, routingKey);

            _logger.Information($"Contact {contact.Id} has been updated successfully!");

            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpDelete("{delete}")]
        public async Task<IActionResult> DeleteContact(Guid contactId)
        {
            var existContact = await _serviceManager.ContactService.GetContactAsync(contactId);

            var routingKey = "DeleteKey";

            _messageProducer.SendMessage(existContact, routingKey);

            _logger.Information($"Contact deleted: {existContact.Id}");

            return Ok();
        }

        [HttpGet("{getById}")]
        public async Task<IActionResult> GetContactById(Guid contactId)
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

        [HttpGet]
        public async Task<IActionResult> GetAllContacts([FromQuery] ContactParameters contactParameters)
        {
            var allContacts = await _serviceManager.ContactService.GetAllContactsAsync(contactParameters);

            _logger.Information("All contacts have been got");

            return Ok(allContacts);
        }
    }
}
