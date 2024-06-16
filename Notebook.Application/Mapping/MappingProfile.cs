using AutoMapper;
using Notebook.Domain.Entities;
using Notebook.Domain.Requests;

namespace Notebook.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<AddressForCreateDTO, Address>()
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.ContactId));
        }
    }
}
