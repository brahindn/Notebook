using Notebook.Application.Services.Contracts.Services;
using Notebook.Domain.Entities;
using Notebook.Repositories.Contracts;

namespace Notebook.Application.Services.Implementation.Services
{
    internal class ContactService : IContactService
    {
        private readonly IRepositoryManager _repositoryManager;

        public ContactService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task CreateContactAsync(string firstName, string lastName, string phoneNumber, string email, DateTime dataOfBirth)
        {
            if(string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(phoneNumber))
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

        public Task<Contact> GetContactAsync(string firstName, string lastName)
        {
            return _repositoryManager.Contact.GetContactAsync(firstName, lastName);
        }
    }
}
