using Notebook.Repositories.Contracts.Repositories;

namespace Notebook.Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IAddressRepository Address { get; }
        IContactRepository Contact { get; }
        Task SaveAsync();
    }
}
