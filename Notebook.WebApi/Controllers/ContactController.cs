using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;
using Notebook.Domain.Entities;

namespace Notebook.WebApi.Controllers
{
    [Route("api/contacts")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _loggerManager;

        public ContactController(IServiceManager serviceManager, ILoggerManager loggerManager)
        {
            _serviceManager = serviceManager;
            _loggerManager = loggerManager;
        }

        [HttpGet("AllContacts")]
        public IActionResult GetContacts()
        {
            try
            {
                _loggerManager.LogInfo($"Getting all contacts.");

                var companies = _serviceManager.ContactService.GetAllContacts();

                _loggerManager.LogInfo($"All contacts are retrieved successfully");

                return Ok(companies);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetContact error: {ex.Message}");

                return StatusCode(500, $"GetContacts error: {ex.Message}");
            }
        }

        [HttpGet("Contact")]
        public async Task<IActionResult> GetContact(Guid id)
        {
            try
            {
                _loggerManager.LogInfo($"Getting contact for id: {id}");

                var company = await _serviceManager.ContactService.GetContactAsync(id);

                if (company == null)
                {
                    _loggerManager.LogError($"Contact for id: {id} not found");

                    return NotFound();
                }

                _loggerManager.LogInfo($"Getting contact for id is successfully.");

                return Ok(company);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetContact error: {ex.Message}");

                return StatusCode(500, $"GetContact error: {ex.Message}");
            }
        }

        [HttpPost("NewContact")]
        public async Task<IActionResult> CreateContact(string firstName, string lastName, string phoneNumber, string email, DateTime dataOfBirth)
        {
            try
            {
                _loggerManager.LogInfo($"Creating a new contact");

                await _serviceManager.ContactService.CreateContactAsync(firstName, lastName, phoneNumber, email, dataOfBirth);

                _loggerManager.LogInfo($"New contact has created successfully.");

                return Ok();
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"CreateContact error: {ex.Message}");

                return StatusCode(500, $"CreateContact error: {ex.Message}");
            }
        }
    }
}
