using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;

namespace Notebook.WebApi.Controllers
{
    [Route("api/contacts")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ContactController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("AllContacts")]
        public IActionResult GetContacts()
        {
            try
            {
                var companies = _serviceManager.ContactService.GetAllContacts();

                return Ok(companies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"GetContacts error: {ex.Message}");
            }
        }

        [HttpGet("Contact")]
        public async Task<IActionResult> GetContact(Guid id)
        {
            try
            {
                var company = await _serviceManager.ContactService.GetContactAsync(id);

                if (company == null)
                {
                    return NotFound();
                }

                return Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"GetContact error: {ex.Message}");
            }
        }

        [HttpPost("NewContact")]
        public async Task<IActionResult> CreateContact(string firstName, string lastName, string phoneNumber, string email, DateTime dataOfBirth)
        {
            try
            {
                await _serviceManager.ContactService.CreateContactAsync(firstName, lastName, phoneNumber, email, dataOfBirth);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"CreateContact error: {ex.Message}");
            }
        }
    }
}
