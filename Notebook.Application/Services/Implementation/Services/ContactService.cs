using Notebook.Application.Services.Contracts.Services;
using Notebook.Domain.Entities;
using Notebook.Repositories.Contracts;
using System.Text.Json;

namespace Notebook.Application.Services.Implementation.Services
{
    public class ContactService : IContactService
    {
        private readonly IRepositoryManager _repositoryManager;

        public ContactService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task CreateContactAsync(string firstName, string lastName, string phoneNumber, string? email, DateTime? dataOfBirth)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return;
            }

            var contact = new Contact
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                DateOfBirth = dataOfBirth
            };

            _repositoryManager.Contact.Create(contact);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateContactAsync(string? newFirstName, string? newLastName, string? newPhoneNumber, string? newEmail, DateTime? newDataOfBirth)
        {
            var existContact = await _repositoryManager.Contact.GetContactByFieldsAsync(newFirstName, newLastName, newPhoneNumber) ?? throw new ArgumentNullException("That contact was not found.");

            existContact.FirstName = newFirstName;
            existContact.LastName = newLastName;
            existContact.PhoneNumber = newPhoneNumber;
            existContact.Email = newEmail;
            existContact.DateOfBirth = newDataOfBirth;

            _repositoryManager.Contact.Update(existContact);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteContactAsync(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }

            _repositoryManager.Contact.Delete(contact);
            await _repositoryManager.SaveAsync();
        }

        public Task<Contact> GetContactAsync(Guid contactId)
        {
            return _repositoryManager.Contact.GetContactAsync(contactId);
        }

        public Task<Contact> GetContactAsyncByFields(string? newFirstName, string? newLastName, string? newPhoneNumber)
        {
            return _repositoryManager.Contact.GetContactByFieldsAsync(newFirstName, newLastName, newPhoneNumber);
        }

        public IQueryable<Contact> GetAllContacts()
        {
            var companies = _repositoryManager.Contact.GetAll();

            return companies;
        }


    }
}
