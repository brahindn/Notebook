using Notebook.Application.Services.Contracts.Services;
using Notebook.Domain.Entities;
using Notebook.Repositories.Contracts;
using Notebook.Shared.RequestFeatures;

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

        public async Task UpdateContactAsync(Guid Id, string? newFirstName, string? newLastName, string? newPhoneNumber, string? newEmail, DateTime? newDataOfBirth)
        {
            var existContact = await _repositoryManager.Contact.GetContactAsync(Id) ?? throw new ArgumentNullException($"That contact {Id} was not found.");

            existContact.FirstName = newFirstName ?? existContact.FirstName;
            existContact.LastName = newLastName ?? existContact.LastName;
            existContact.PhoneNumber = newPhoneNumber ?? existContact.PhoneNumber;
            existContact.Email = newEmail ?? existContact.Email;
            existContact.DateOfBirth = newDataOfBirth ?? existContact.DateOfBirth;

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

        public async Task<IEnumerable<Contact>> GetAllContactsAsync(ContactParameters contactParameters)
        {
            return await _repositoryManager.Contact.GetContactsAsync(contactParameters);
        }

        public async Task<Contact> GetContactByFieldAsync(string? firstName, string? lastName, string? phoneNumber, string? email)
        {
            return await _repositoryManager.Contact.GetContactByFieldsAsync(firstName, lastName, phoneNumber, email);
        }
    }
}
