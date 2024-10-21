using Notebook.Domain.Entities;
using Notebook.Domain.Requests;
using Notebook.Shared.RequestFeatures;

namespace Notebook.Repositories.Contracts.Repositories
{
    public interface IContactRepository
    {
        void Create(Contact contact);
        void Update(Contact contact);
        void Delete(Contact contact);
        Task<Contact> GetContactAsync(Guid id);
        Task<IEnumerable<Contact>> GetContactByFieldsAsync(IQueryable<Contact> query);
        IQueryable<Contact> GetAll();
        Task<IEnumerable<Contact>> GetContactsAsync(ContactParameters contactParameters);
    }
}
