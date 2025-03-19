using AutoMapper;
using FabriqPro.Features.Authentication.DTOs;
using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Authentication.Profiles;

public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<UserCreateDto, User>();
        CreateMap<User, UserListDto>().ForMember(dest => dest.FullName,
            opts => opts.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}