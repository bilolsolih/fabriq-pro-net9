using AutoMapper;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Material;

namespace FabriqPro.Features.Products.Profiles;

public class MaterialProfiles : Profile
{
  public MaterialProfiles()
  {
    CreateMap<MaterialTypeCreateDto, MaterialType>();
    CreateMap<Material, MaterialsListAllDto>()
      .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.MaterialType.Title))
      .ForMember(dest => dest.Quantity, opts => opts.MapFrom(src => $"{src.Quantity} {GetTitleForUnit(src.Unit)}"));

    CreateMap<MaterialType, MaterialTypeListDto>()
      .ForMember(
        dest => dest.TotalInMeter,
        opts => opts.MapFrom(src => src.Materials.Where(md => md.Unit == Unit.Meter).Sum(md => md.Quantity))
      ).ForMember(
        dest => dest.TotalInKg,
        opts => opts.MapFrom(src => src.Materials.Where(md => md.Unit == Unit.Kg).Sum(md => md.Quantity))
      ).ForMember(
        dest => dest.TotalInPack,
        opts => opts.MapFrom(src => src.Materials.Where(md => md.Unit == Unit.Piece).Sum(md => md.Quantity))
      );

    CreateMap<Material, MaterialListDto>()
      .ForMember(dest => dest.PartyNumber, opts => opts.MapFrom(src => src.Party.Title))
      .ForMember(dest => dest.FromUser, opts => opts.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}"))
      .ForMember(dest => dest.FromUserRole, opts => opts.MapFrom(src => src.FromUser.Role))
      .ForMember(dest => dest.ToUser, opts => opts.MapFrom(src => $"{src.ToUser.FirstName} {src.ToUser.LastName}"))
      .ForMember(dest => dest.ToUserRole, opts => opts.MapFrom(src => src.ToUser.Role))
      .ForMember(dest => dest.AcceptedUser, opts => opts.MapFrom(src => $"{src.AcceptedUser.FirstName} {src.AcceptedUser.LastName}"))
      .ForMember(dest => dest.AcceptedUserRole, opts => opts.MapFrom(src => src.AcceptedUser.Role))
      .ForMember(dest => dest.Color, opts => opts.MapFrom(src => src.Color.ColorCode))
      .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.Created));

    CreateMap<Material, MaterialFlowListDto>()
      .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.MaterialType.Title))
      .ForMember(dest => dest.PartyNumber, opts => opts.MapFrom(src => src.Party.Title))
      .ForMember(dest => dest.FromUser, opts => opts.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}"))
      .ForMember(dest => dest.FromUserRole, opts => opts.MapFrom(src => src.FromUser.Role))
      .ForMember(dest => dest.Color, opts => opts.MapFrom(src => src.Color.ColorCode))
      .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.Created));
  }

  public string GetTitleForUnit(Unit unit)
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