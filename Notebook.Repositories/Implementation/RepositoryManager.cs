using Notebook.DataAccess;
using Notebook.Repositories.Contracts;
using Notebook.Repositories.Contracts.Repositories;
using Notebook.Repositories.Implementation.Repositories;

namespace Notebook.Repositories.Implementation
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;

        private readonly Lazy<IContactRepository> _contactRepository;
        private readonly Lazy<IAddressRepository> _addressRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _contactRepository = new Lazy<IContactRepository>(() => new ContactRepository(repositoryContext));
            _addressRepository = new Lazy<IAddressRepository>(() => new AddressRepository(repositoryContext));
        }

        public IContactRepository Contact => _contactRepository.Value;
        public IAddressRepository Address => _addressRepository.Value;

        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();    
        }
    }
}
