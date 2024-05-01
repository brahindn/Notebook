using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
using Notebook.Shared.RequestFeatures;
using Notebook.WebApi.RabbitMQ;
using Notebook.WebApi.Requests;
using Notebook.WebApi.Responses;

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
        public async Task<IActionResult> AddContact([FromBody] ContactForCreateDTO contact)
        {
            if (contact == null)
            {
                return BadRequest("ContactForCreateUpdateDTO object is null");
            }

            try
            {
                var routingKey = "AddKey";

                _messageProducer.SendMessage(contact, routingKey);

                _logger.Information($"New contact {contact.FirstName} {contact.LastName} has been added successfully");

                return Ok(); 
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"CreateContact error: {ex.Message}");
            }
        }

        [HttpPut("{update}")]
        public async Task<IActionResult> UpdateContact([FromBody] ContactForUpdateDTO contact)
        {
            if (contact == null)
            {
                return BadRequest("ContactForCreateUpdateDTO object is null");
            }

            try
            {
                var routingKey = "UpdateKey";

                _messageProducer.SendMessage(contact, routingKey);

                _logger.Information($"Contact {contact.Id} has been updated successfully!");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"UpdateContact error: {ex.Message}");
            }
        }

        [HttpDelete("{delete}")]
        public async Task<IActionResult> DeleteContact(Guid contactId)
        {
            var existContact = await _serviceManager.ContactService.GetContactAsync(contactId);

            if (existContact == null)
            {
                return NotFound();
            }

            try
            {
                var routingKey = "DeleteKey";

                _messageProducer.SendMessage(existContact, routingKey);

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

        [HttpGet]
        public async Task<IActionResult> GetAllContacts([FromQuery] ContactParameters contactParameters)
        {
            try
            {
                var allContacts = await _serviceManager.ContactService.GetAllContactsAsync(contactParameters);

                _logger.Information("All contacts have been got");

                return Ok(allContacts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"GetContacts error: {ex.Message}");
            }
        }
    }
}
