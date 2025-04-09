using AutoMapper;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.Products.Profiles;

public class MaterialTypeProfiles : Profile
{
  public MaterialTypeProfiles()
  {
    CreateMap<MaterialType, MaterialTypeListDto>();
    CreateMap<MaterialType, MaterialTypeDetailDto>();
    CreateMap<MaterialTypeCreateDto, MaterialType>()
      .ForMember(dest => dest.ProductTypes, opts => opts.Ignore());
  }
}