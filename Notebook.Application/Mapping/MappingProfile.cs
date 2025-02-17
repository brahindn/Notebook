using AutoMapper;
using Notebook.Domain;
using Notebook.Domain.Entities;
using Notebook.Domain.Requests;
using Notebook.Domain.Responses;

namespace Notebook.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateContactRequest, Contact>();

            CreateMap<CreateContactRequest, Contact>()
                .ForMember(dest => dest.FirstName, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.FirstName)))
                .ForMember(dest => dest.LastName, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.LastName)))
                .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.PhoneNumber)))
                .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Email)))
                .ForMember(dest => dest.DateOfBirth, opt => opt.Condition(src => src.DateOfBirth != null));

            CreateMap<Contact, GetContactResponse>();

            CreateMap<UpdateContactRequest, Contact>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Contact, UpdateContactRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<CreateAddressRequest, Address>()
                .ForMember(dest => dest.Contact, opt => opt.Ignore());

            CreateMap<Address, GetAddressResponse>();

            CreateMap<UpdateAddressRequest, Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Address, UpdateAddressRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<GetContactResponse, UpdateContactRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
