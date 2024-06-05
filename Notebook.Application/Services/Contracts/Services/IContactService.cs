using Notebook.Domain.Entities;
using Notebook.Shared.RequestFeatures;

namespace Notebook.Application.Services.Contracts.Services
{
    public interface IContactService
    {
        Task CreateContactAsync(string firstName, string lastName, string phoneNumber, string? email, DateTime? dataOfBirth);
        Task UpdateContactAsync(Guid Id, string? newFirstName, string? newLastName, string? newPhoneNumber, string? newEmail, DateTime? newDataOfBirth);
        Task DeleteContactAsync(Contact contact);
        Task<Contact> GetContactAsync(Guid id);
        Task<IEnumerable<Contact>> GetAllContactsAsync(ContactParameters contactParameters);
        Task<Contact> GetContactByFieldAsync(string? firstName, string? lastName, string? phoneNumber, string? email);
    }
}
    