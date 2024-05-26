using AddressBook.DataTransferModels;
using AddressBook.Models;
using AutoMapper;

namespace AddressBook.Common;

public class CustomProfile : Profile
{
    public CustomProfile()
    {
        CreateMap<UserModel, UserDataDTM>()
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.Address.Zip))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country))
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Address.CountryCode))
            .ForMember(dest => dest.CountryFlagUrl, opt => opt.MapFrom(src => src.Address.CountryFlagUrl))
            .ForMember(dest => dest.PhoneNumberCode, opt => opt.MapFrom(src => src.PhoneNumberCode))
            .ReverseMap();

        CreateMap<UserModel, UserDataPostDTM>()
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.Address.Zip))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country))
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Address.CountryCode))
            .ForMember(dest => dest.CountryFlagUrl, opt => opt.MapFrom(src => src.Address.CountryFlagUrl))
            .ForMember(dest => dest.PhoneNumberCode, opt => opt.MapFrom(src => src.PhoneNumberCode))
            .ReverseMap();

        CreateMap<UserDataPostDTM, UserDataDTM>()
            .ReverseMap();

        CreateMap<UserModel, UserModel>();
    }
}
