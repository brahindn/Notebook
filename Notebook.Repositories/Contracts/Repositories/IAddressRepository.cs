using Notebook.Domain.Entities;

namespace Notebook.Repositories.Contracts.Repositories
{
    public interface IAddressRepository
    {
        void Create(Address address);
        void Update(Address address);
        void Delete(Address address);
        Task<Address> GetAddressAsync(Guid contactId);
        IQueryable<Address> GetAll();
    }
}
