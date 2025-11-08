using AutoMapper;
using Firmeza.API.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.API.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<IdentityUser, UserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap();
        }
    }
}
