using Notebook.Domain.Entities;
using Notebook.Domain.Requests;
using Notebook.Shared.RequestFeatures;

namespace Notebook.Application.Services.Contracts.Services
{
    public interface IContactService
    {
        Task CreateContactAsync(ContactForCreateDTO contactDTO);
        Task UpdateContactAsync(ContactForUpdateDTO contactDTO);
        Task DeleteContactAsync(Contact contact);
        Task<Contact> GetContactAsync(Guid id);
        Task<IEnumerable<Contact>> GetAllContactsAsync(ContactParameters contactParameters);
        Task<Contact> GetContactByFieldAsync(string? firstName, string? lastName, string? phoneNumber, string? email);
    }
}
    