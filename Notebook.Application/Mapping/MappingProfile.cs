using AutoMapper;
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

            CreateMap<CreateAddressRequest, Address>();
            CreateMap<Address, GetAddressResponse>();

            /*CreateMap<AddressForCreateDTO, Address>()
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.ContactId));

            CreateMap<AddressForUpdateDTO, Address>()
                .ForMember(dest => dest.AddressType, opt => opt.Condition(src => src.AddressType != null))
                .ForMember(dest => dest.Country, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Country)))
                .ForMember(dest => dest.Region, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Region)))
                .ForMember(dest => dest.City, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.City)))
                .ForMember(dest => dest.Street, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Street)))
                .ForMember(dest => dest.BuildingNumber, opt => opt.Condition(src => src.BuildingNumber != null && src.BuildingNumber != 0));

            

            CreateMap<Contact, GetContactResponse>();

            CreateMap<Contact, GetContactResponse>()
                .ForMember(dest => dest.FirstName, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.FirstName)))
                .ForMember(dest => dest.LastName, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.LastName)))
                .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.PhoneNumber)))
                .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Email)))
                .ForMember(dest => dest.DateOfBirth, opt => opt.Condition(src => src.DateOfBirth != null));


            CreateMap<GetContactResponse, CreateContactRequest>();

            CreateMap<ContactForUpdateDTO, Contact>()
                .ForMember(dest => dest.FirstName, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.FirstName)))
                .ForMember(dest => dest.LastName, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.LastName)))
                .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.PhoneNumber)))
                .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Email)))
                .ForMember(dest => dest.DateOfBirth, opt => opt.Condition(src => src.DateOfBirth != null));

            CreateMap<List<Contact>, List<GetContactResponse>>();*/
        }
    }
}
