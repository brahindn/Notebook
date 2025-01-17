using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
using Notebook.Shared.RequestFeatures;
using Notebook.WebApi.RabbitMQ;
using Notebook.Domain.Requests;
using AutoMapper;

namespace Notebook.WebApi.Controllers
{
    [Route("api/contacts/")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly Serilog.ILogger _logger;
        private readonly MessageProducer _messageProducer;
        private readonly IMapper _mapper;

        public ContactController(IServiceManager serviceManager, Serilog.ILogger logger, MessageProducer messageProducer, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            _messageProducer = messageProducer;
            _mapper = mapper;
        }

        [HttpPost("add")]
        public Task<IActionResult> AddContact([FromBody] CreateContactRequest contact)
        {

            var routingKey = "AddContactKey";

            _messageProducer.SendMessage(contact, routingKey);

            _logger.Information($"New contact {contact.FirstName} {contact.LastName} has been added successfully");

            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpPut("update")]
        public Task<IActionResult> UpdateContact([FromBody] UpdateContactRequest contact)
        {
            var routingKey = "UpdateContactKey";

            _messageProducer.SendMessage(contact, routingKey);

            _logger.Information($"Contact {contact.Id} has been updated successfully!");

            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteContact(Guid contactId)
        {
            var existContact = await _serviceManager.ContactService.GetContactByIdAsync(contactId);

            var routingKey = "DeleteContactKey";

            _messageProducer.SendMessage(existContact, routingKey);

            _logger.Information($"Contact deleted: {existContact.Id}");

            return Ok();
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetContactById(Guid contactId)
        {
            var contact = await _serviceManager.ContactService.GetContactByIdAsync(contactId);

            if (contact == null)
            {
                return NotFound();
            }

            var updateContactRequest = _mapper.Map<UpdateContactRequest>(contact);

            _logger.Information($"Contact {updateContactRequest.Id} has been got");

            return Ok(updateContactRequest);
        }

        [HttpPost("getByFields")]
        public async Task<IActionResult> GetContactsByFields([FromBody]GetContactRequest contactRequest)
        {
            if (contactRequest == null)
            {
                return BadRequest();
            }

            var allContacts = await _serviceManager.ContactService.GetContactByFieldAsync(contactRequest);

            _logger.Information("Neсessary contacts received");

            return Ok(allContacts);
        }

        [HttpGet("getByPhoneNumber")]
        public async Task<IActionResult> GetContactsByFields(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return BadRequest();
            };

            var contactRequest = new GetContactRequest();
            contactRequest.PhoneNumber = phoneNumber;

            var allContacts = await _serviceManager.ContactService.GetContactByFieldAsync(contactRequest);

            _logger.Information("Neсessary contacts received");

            return Ok(allContacts);
        }

        [HttpGet("allContacts")]
        public async Task<IActionResult> GetAllContacts([FromQuery] ContactParameters contactParameters)
        {
            var allContacts = await _serviceManager.ContactService.GetAllContactsAsync(contactParameters);

            _logger.Information("All contacts received");

            return Ok(allContacts);
        }
    }
}
