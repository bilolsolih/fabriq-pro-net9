using AutoMapper;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.Products.Profiles;

public class ProductTypeProfiles : Profile
{
  public ProductTypeProfiles()
  {
    CreateMap<ProductType, ProductTypeDetailDto>()
      .ForMember(dest => dest.Created, opts => opts.MapFrom(src => src.Created.ToLocalTime()))
      .ForMember(dest => dest.Updated, opts => opts.MapFrom(src => src.Updated.ToLocalTime()));
    CreateMap<ProductType, ProductTypeListDto>();
    CreateMap<ProductTypeCreateDto, ProductType>();
    CreateMap<ProductTypeUpdateDto, ProductType>()
      .ForAllMembers(
        opts => opts.Condition(
          (src, dest, obj) =>
          {
            if (obj is string str)
            {
              return !string.IsNullOrEmpty(str);
            }

            return obj != null;
          }
        )
      );
  }
}