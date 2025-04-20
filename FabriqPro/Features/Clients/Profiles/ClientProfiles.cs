using AutoMapper;
using FabriqPro.Features.Clients.DTOs;
using FabriqPro.Features.Clients.Models;

namespace FabriqPro.Features.Clients.Profiles;

public class ClientProfiles : Profile
{
    public ClientProfiles()
    {
        CreateMap<ClientCreateDto, Client>();
        CreateMap<Client, ClientListDto>()
            .ForMember(dest => dest.PurchasesCount, opts => opts.MapFrom(src => 0));
        CreateMap<Client, ClientDetailDto>()
            .ForMember(dest => dest.Created, opts => opts.MapFrom(src => src.Created.ToLocalTime()))
            .ForMember(dest => dest.PurchasesCount, opts => opts.MapFrom(src => 0));
        CreateMap<ClientUpdateDto, Client>().ForAllMembers(
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