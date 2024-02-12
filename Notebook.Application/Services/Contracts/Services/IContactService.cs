using Notebook.Domain.Entities;

namespace Notebook.Application.Services.Contracts.Services
{
    public interface IContactService
    {
        Task CreateContactAsync(string firstName, string lastName, string phoneNumber, string email, DateTime dataOfBirth);
        Task<Contact> GetContactAsync(string firstName, string lastName);
    }
}
