using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;

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
        public async Task<IActionResult> AddContact(string firstName, string lastName, string phoneNumber, string email, DateTime dataOfBirth)
        {
            try
            {
                await _serviceManager.ContactService.CreateContactAsync(firstName, lastName, phoneNumber, email, dataOfBirth);

                _logger.Information($"New contact {firstName} {lastName} has been added successfully");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"CreateContact error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(Guid id, string newFirstName, string newLastName, string newPhoneNumber, string newEmail, DateTime newDataOfBirth)
        {
            try
            {
                var existContact = await _serviceManager.ContactService.GetContactAsync(id);

                if (existContact == null)
                { 
                    return NotFound();
                }

                await _serviceManager.ContactService.UpdateContactAsync(id, newFirstName, newLastName, newPhoneNumber, newEmail, newDataOfBirth);

                _logger.Information($"Contact {id} has been updated successfully!");
            }
            catch(Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"UpdateContact error: {ex.Message}");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            var existContact = await _serviceManager.ContactService.GetContactAsync(id);

            if (existContact == null)
            {
                return NotFound();
            }

            try
            {
                await _serviceManager.ContactService.DeleteContactAsync(existContact);

                _logger.Information($"Contact deleted: {existContact.Id}");
            }
            catch(Exception ex)
            {
                _logger.Error(ex.InnerException.Message);

                return StatusCode(500, $"DeleteContact error: {ex.Message}");
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(Guid id)
        {
            try
            {
                var contact = await _serviceManager.ContactService.GetContactAsync(id);

                if (contact == null)
                {
                    return NotFound();
                }

                _logger.Information($"Contact {contact.Id} has been got");

                return Ok(contact);
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
