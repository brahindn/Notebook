﻿using Microsoft.AspNetCore.Mvc;
using Notebook.Application.Services.Contracts;

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

        [HttpPost]
        public async Task<IActionResult> AddContact(string firstName, string lastName, string phoneNumber, string email, DateTime dataOfBirth)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(Guid id, string newFirstName, string newLastName, string newPhoneNumber, string newEmail, DateTime newDataOfBirth)
        {
            var existContact = await _serviceManager.ContactService.GetContactAsync(id);

            if (existContact == null)
            {
                return NotFound();
            }

            try
            {
                await _serviceManager.ContactService.UpdateContactAsync(id, newFirstName, newLastName, newPhoneNumber, newEmail, newDataOfBirth);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"UpdateContact error: {ex.Message}");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            var existContact = await _serviceManager.ContactService.GetContactAsync(id) ?? throw new ArgumentException($"Contact with id: {id} not found.");

            try
            {
                await _serviceManager.ContactService.DeleteContactAsync(existContact);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"DeleteContact error: {ex.Message}");
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(Guid id)
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

        [HttpGet]
        public IActionResult GetAllContacts()
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
    }
}
