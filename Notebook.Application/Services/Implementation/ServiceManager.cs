﻿using AutoMapper;
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
        private readonly Lazy<IMongoService> _mongoService;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _contactService = new Lazy<IContactService>(() => new ContactService(repositoryManager, mapper));
            _addressService = new Lazy<IAddressService>(() => new AddressService(repositoryManager, mapper));
            _mongoService = new Lazy<IMongoService>(() => new MongoService());
        }
        
        public IContactService ContactService => _contactService.Value;
        public IAddressService AddressService => _addressService.Value;
        public IMongoService MongoService => _mongoService.Value;
    }
}
