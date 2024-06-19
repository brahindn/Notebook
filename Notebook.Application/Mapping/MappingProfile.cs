using AutoMapper;
using MongoDB.Driver.Core.Authentication;
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

            CreateMap<AddressForUpdateDTO, Address>()
                .ForMember(dest => dest.AddressType, opt => opt.Condition(src => src.AddressType != null))
                .ForMember(dest => dest.Country, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Country)))
                .ForMember(dest => dest.Region, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Region)))
                .ForMember(dest => dest.City, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.City)))
                .ForMember(dest => dest.Street, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Street)))
                .ForMember(dest => dest.BuildingNumber, opt => opt.Condition(src => src.BuildingNumber != null && src.BuildingNumber != 0));
        }
    }
}
