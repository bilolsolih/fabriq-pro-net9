using AutoMapper;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.Products.Profiles;

public class MaterialProfiles : Profile
{
  public MaterialProfiles()
  {
    CreateMap<MaterialCreateDto, Material>();
    CreateMap<Material, MaterialListDto>()
      .ForMember(dest => dest.MaterialType, opts => opts.MapFrom(src => src.MaterialType.Title))
      .ForMember(dest => dest.ProductType, opts => opts.MapFrom(src => src.ProductType.Title));
  }
}