using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notebook.Application.Services.Contracts.Services;
using Notebook.Domain.Entities;
using Notebook.Domain.Requests;
using Notebook.Domain.Responses;
using Notebook.Repositories.Contracts;
using Notebook.Shared.RequestFeatures;

namespace Notebook.Application.Services.Implementation.Services
{
    public class ContactService : IContactService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ContactService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task CreateContactAsync(CreateContactRequest createContactRequest)
        {
            var contact = _mapper.Map<Contact>(createContactRequest);

            _repositoryManager.Contact.Create(contact);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateContactAsync(Guid contactId, UpdateContactRequest updateContactRequest)
        {
            var contact = await _repositoryManager.Contact.GetContactByIdAsync(contactId);

            if (contact == null || updateContactRequest == null)
            {
                throw new ArgumentNullException();
            }

            if(string.IsNullOrEmpty(updateContactRequest.FirstName) &&
               string.IsNullOrEmpty(updateContactRequest.LastName) &&
               string.IsNullOrEmpty(updateContactRequest.PhoneNumber) &&
               string.IsNullOrEmpty(updateContactRequest.Email) &&
               !updateContactRequest.DateOfBirth.HasValue)
            {
                throw new ArgumentException();
            }

            var updatedContact = _mapper.Map(updateContactRequest, contact);

            _repositoryManager.Contact.Update(updatedContact);
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

        public Task<Contact> GetContactByIdAsync(Guid contactId)
        {
            return _repositoryManager.Contact.GetContactByIdAsync(contactId);
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync(ContactParameters contactParameters)
        {
            return await _repositoryManager.Contact.GetContactsAsync(contactParameters);
        }

        public async Task<IEnumerable<GetContactResponse>> GetContactByFieldAsync(GetContactRequest contactRequest)
        {
            var query = _repositoryManager.Contact.GetAll();

            if (!string.IsNullOrEmpty(contactRequest.FirstName))
            {
                query = query.Where(c => c.FirstName == contactRequest.FirstName);
            }
            if (!string.IsNullOrEmpty(contactRequest.LastName))
            {
                query = query.Where(c => c.LastName == contactRequest.LastName);
            }
            if (!string.IsNullOrEmpty(contactRequest.PhoneNumber))
            {
                query = query.Where(c => c.PhoneNumber == contactRequest.PhoneNumber);
            }
            if (!string.IsNullOrEmpty(contactRequest.Email))
            {
                query = query.Where(c => c.Email == contactRequest.Email);
            }

            var contacts = await query.ToListAsync();

            return _mapper.Map<IEnumerable<GetContactResponse>>(contacts);
        }
    }
}
