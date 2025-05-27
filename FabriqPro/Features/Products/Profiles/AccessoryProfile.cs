using AutoMapper;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Accessory;

namespace FabriqPro.Features.Products.Profiles;

public class AccessoryProfile : Profile
{
  public AccessoryProfile()
  {

    CreateMap<AccessoryType, AccessoryTypeEntryListDto>();
    CreateMap<AccessoryType, AccessoryTypeListDto>()
      .ForMember(dest => dest.Quantity, opts => opts.MapFrom(src => src.Accessories.Sum(a => a.Quantity)));
      // .ForMember(dest => dest.Unit, opts => opts.MapFrom(src => Utils.GetTitleForUnit(Unit.Piece)));

    CreateMap<Accessory, AccessoryListDto>()
      .ForMember(dest => dest.FromUser, opts => opts.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}"))
      .ForMember(dest => dest.FromUserRole, opts => opts.MapFrom(src => src.FromUser.Role))
      .ForMember(dest => dest.ToUser, opts => opts.MapFrom(src => $"{src.ToUser.FirstName} {src.ToUser.LastName}"))
      .ForMember(dest => dest.ToUserRole, opts => opts.MapFrom(src => src.ToUser.Role))
      .ForMember(dest => dest.AcceptedUser, opts => opts.MapFrom(src => $"{src.AcceptedUser.FirstName} {src.AcceptedUser.LastName}"))
      .ForMember(dest => dest.AcceptedUserRole, opts => opts.MapFrom(src => src.AcceptedUser.Role))
      .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.Created));

    CreateMap<Accessory, AccessoryFlowListDto>()
      .ForMember(dest => dest.FromUser, opts => opts.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}"))
      .ForMember(dest => dest.FromUserRole, opts => opts.MapFrom(src => src.FromUser.Role))
      .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.Created));
  }
}