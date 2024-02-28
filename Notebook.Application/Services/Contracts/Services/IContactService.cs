using Notebook.Domain.Entities;

namespace Notebook.Application.Services.Contracts.Services
{
    public interface IContactService
    {
        Task CreateContactAsync(string firstName, string lastName, string phoneNumber, string? email, DateTime? dataOfBirth);
        Task UpdateContactAsync(Guid id, string? newFirstName, string? newLastName, string? newPhoneNumber, string? newEmail, DateTime? newDataOfBirth);
        Task DeleteContactAsync(Contact contact);
        Task<Contact> GetContactAsync(Guid id);
        IQueryable<Contact> GetAllContacts();
    }
}
    