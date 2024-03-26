using Notebook.Domain.Entities;

namespace Notebook.Repositories.Contracts.Repositories
{
    public interface IContactRepository
    {
        void Create(Contact contact);
        void Update(Contact contact);
        void Delete(Contact contact);
        Task<Contact> GetContactAsync(Guid id);
        Task<Contact> GetContactByFieldsAsync(string newFirstName, string newLastName, string newPhoneNumber);
        IQueryable<Contact> GetAll();
    }
}
