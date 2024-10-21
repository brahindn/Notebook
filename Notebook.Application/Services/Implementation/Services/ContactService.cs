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

        public async Task UpdateContactAsync(ContactForUpdateDTO contactDTO)
        {
            var existContact = await _repositoryManager.Contact.GetContactAsync(contactDTO.Id) ?? throw new ArgumentNullException($"That contact {contactDTO.Id} was not found.");

            _mapper.Map(contactDTO, existContact);

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

            //var contacts = await _repositoryManager.Contact.GetContactByFieldsAsync(query);
            var contacts = await query.ToListAsync();

            return _mapper.Map<IEnumerable<GetContactResponse>>(contacts);
        }
    }
}
