using AutoMapper;
using FabriqPro.Features.Authentication.DTOs;
using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Authentication.Profiles;

public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<UserCreateDto, User>();
        CreateMap<User, UserListDto>().ForMember(
            dest => dest.FullName,
            opts => opts.MapFrom(src => $"{src.FirstName} {src.LastName}")
        );

        CreateMap<User, UserDetailDto>();
        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.Role, opts => opts.MapFrom((src, dest) => src.Role ?? dest.Role))
            .ForAllMembers(
                opts => opts.Condition(
                    (dto, user, dtoMember) =>
                    {
                        if (dtoMember is string str)
                        {
                            return !string.IsNullOrEmpty(str);
                        }

                        return dtoMember != null;
                    }
                )
            );
    }
}