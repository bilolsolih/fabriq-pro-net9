using AutoMapper;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models.Miscellaneous;

namespace FabriqPro.Features.Products.Profiles;

public class MiscellaneousProfile : Profile
{
  public MiscellaneousProfile()
  {
    CreateMap<MiscellaneousType, MiscellaneousTypeListDto>();
    CreateMap<Miscellaneous, MiscellaneousListDto>()
      .ForMember(dest => dest.FromUser, opts => opts.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}"))
      .ForMember(dest => dest.FromUserRole, opts => opts.MapFrom(src => src.FromUser.Role))
      .ForMember(dest => dest.ToUser, opts => opts.MapFrom(src => $"{src.ToUser.FirstName} {src.ToUser.LastName}"))
      .ForMember(dest => dest.ToUserRole, opts => opts.MapFrom(src => src.ToUser.Role))
      .ForMember(dest => dest.AcceptedUser, opts => opts.MapFrom(src => $"{src.AcceptedUser.FirstName} {src.AcceptedUser.LastName}"))
      .ForMember(dest => dest.AcceptedUserRole, opts => opts.MapFrom(src => src.AcceptedUser.Role))
      .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.Created));
    
    CreateMap<Miscellaneous, MiscellaneousFlowListDto>()
      .ForMember(dest => dest.FromUser, opts => opts.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}"))
      .ForMember(dest => dest.FromUserRole, opts => opts.MapFrom(src => src.FromUser.Role))
      .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.Created));
  }
  
}