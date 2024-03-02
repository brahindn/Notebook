using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
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

        public ContactController(IServiceManager serviceManager, Serilog.ILogger logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] ContactForCreateUpdateDTO contact)
        {
            if(contact == null)
            {
                return BadRequest("ContactForCreateUpdateDTO object is null");
            }

            try
            {
                await _serviceManager.ContactService.CreateContactAsync(contact.FirstName, contact.LastName, contact.PhoneNumber, contact.Email, contact.DateOfBirth);

                _logger.Information($"New contact {contact.FirstName} {contact.LastName} has been added successfully");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"CreateContact error: {ex.Message}");
            }
        }

        [HttpPut("{contactId}")]
        public async Task<IActionResult> UpdateContact(Guid contactId, [FromBody] ContactForCreateUpdateDTO contact)
        {
            if(contact == null)
            {
                return BadRequest("ContactForCreateUpdateDTO object is null");
            }

            try
            {
                var existContact = await _serviceManager.ContactService.GetContactAsync(contactId);

                if (existContact == null)
                { 
                    return NotFound();
                }

                await _serviceManager.ContactService.UpdateContactAsync(contactId, contact.FirstName, contact.LastName, contact.PhoneNumber, contact.Email, contact.DateOfBirth);

                _logger.Information($"Contact {contactId} has been updated successfully!");

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"UpdateContact error: {ex.Message}");
            }
        }

        [HttpDelete("{contactId}")]
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
            catch(Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"DeleteContact error: {ex.Message}");
            }
        }

        [HttpGet("{contactId}")]
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
    }
}
