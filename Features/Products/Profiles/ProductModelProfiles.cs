using AutoMapper;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.Products.Profiles;

public class ProductModelProfiles : Profile
{
  public ProductModelProfiles()
  {
    CreateMap<ProductModel, ProductModelDetailDto>()
      .ForMember(dest => dest.Created, opts => opts.MapFrom(src => src.Created.ToLocalTime()))
      .ForMember(dest => dest.Updated, opts => opts.MapFrom(src => src.Updated.ToLocalTime()));
    CreateMap<ProductModel, ProductModelListDto>()
      .ForMember(dest => dest.Color, opts => opts.MapFrom(src => src.Color.ColorCode))
      .ForMember(dest => dest.ProductType, opts => opts.MapFrom(src => src.ProductType.Title));
    CreateMap<ProductModelCreateDto, ProductModel>();
    CreateMap<ProductModelUpdateDto, ProductModel>()
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