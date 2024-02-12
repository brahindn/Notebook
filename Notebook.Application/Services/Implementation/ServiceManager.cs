using Notebook.Application.Services.Contracts;
using Notebook.Application.Services.Contracts.Services;
using Notebook.Application.Services.Implementation.Services;
using Notebook.Repositories.Contracts;

namespace Notebook.Application.Services.Implementation
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IContactService> _contactService;
        private readonly Lazy<IAddressService> _addressService;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _contactService = new Lazy<IContactService>(() => new ContactService(repositoryManager));
            _addressService = new Lazy<IAddressService>(() => new AddressService(repositoryManager));
        }
        
        public IContactService ContactService => _contactService.Value;
        public IAddressService AddressService => _addressService.Value;
    }
}
