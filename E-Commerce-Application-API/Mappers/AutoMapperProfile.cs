using AutoMapper;
using E_Commerce_Application_API.DTOs;
using E_Commerce_Application_API.Models;

namespace E_Commerce_Application_API.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDTO, User>()
            .ForMember(dest => dest.RegisteredDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ReverseMap();
        }

    }
}
