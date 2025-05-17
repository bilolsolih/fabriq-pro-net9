using AutoMapper;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Miscellaneous;

namespace FabriqPro.Features.Products.Profiles;

public class MiscellaneousProfile : Profile
{
  public MiscellaneousProfile()
  {
    CreateMap<MiscellaneousType, MiscellaneousTypeListDto>();
    CreateMap<Miscellaneous, MiscellaneousListAllDto>()
      .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.MiscellaneousType.Title))
      .ForMember(dest => dest.Quantity, opts => opts.MapFrom(src => $"{src.Quantity} {GetTitleForUnit(src.Unit)}"));
    
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
  
  public static string GetTitleForUnit(Unit unit)
  {
    var map = new Dictionary<Unit, string>
    {
      { Unit.Piece, "dona" },
      { Unit.Kg, "kg" },
      { Unit.Meter, "metr" },
    };

    return map[unit];
  }
  
}