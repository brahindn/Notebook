using Notebook.Application.Services.Contracts.Services;

namespace Notebook.Application.Services.Contracts
{
    public interface IServiceManager
    {
        IContactService ContactService { get; }
        IAddressService AddressService { get; }
    }
}
