using Notebook.Domain.Entities;
using Notebook.Domain.Requests;
using Notebook.Domain.Responses;
using Notebook.Shared.RequestFeatures;

namespace Notebook.Application.Services.Contracts.Services
{
    public interface IContactService
    {
        Task CreateContactAsync(CreateContactRequest createContactRequest);
        Task UpdateContactAsync(ContactForUpdateDTO contactDTO);
        Task DeleteContactAsync(Contact contact);
        Task<Contact> GetContactAsync(Guid id);
        Task<IEnumerable<Contact>> GetAllContactsAsync(ContactParameters contactParameters);
        Task<IEnumerable<GetContactResponse>> GetContactByFieldAsync(GetContactRequest contactRequest);
    }
}
    