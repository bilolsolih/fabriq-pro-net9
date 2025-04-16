using AutoMapper;
using FabriqPro.Features.ProductParts.DTOs;
using FabriqPro.Features.ProductParts.Models;

namespace FabriqPro.Features.ProductParts.Profiles;

public class ProductPartProfiles : Profile
{
    public ProductPartProfiles()
    {
        CreateMap<ProductPart, ProductPartDetailDto>();

        CreateMap<ProductPart, ProductPartListDto>()
            .ForMember(dest => dest.ProductTypeName, opts => opts.MapFrom(src => src.ProductType.Title))
            .ForMember(dest => dest.ProductPartTypeName, opts => opts.MapFrom(src => src.ProductPartType.Title))
            .ForMember(dest => dest.ProductModelName, opts => opts.MapFrom(src => src.ProductModel.Title));

        CreateMap<ProductPartCreateDto, ProductPart>();

        CreateMap<ProductPartUpdateDto, ProductPart>()
            .ForAllMembers(
                opts => opts.Condition(
                    (src, dest, obj) =>
                    {
                        return obj != null;
                    }
                )
            );
    }
}