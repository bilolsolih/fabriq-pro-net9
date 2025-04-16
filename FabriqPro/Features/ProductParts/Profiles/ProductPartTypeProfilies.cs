using AutoMapper;
using FabriqPro.Features.ProductParts.DTOs;
using FabriqPro.Features.ProductParts.Models;

namespace FabriqPro.Features.ProductParts.Profiles;

public class ProductPartTypeProfiles : Profile
{
    public ProductPartTypeProfiles()
    {
        CreateMap<ProductPartType, ProductPartTypeDetailDto>()
            .ForMember(dest => dest.Created, opts => opts.MapFrom(src => src.Created.ToLocalTime()))
            .ForMember(dest => dest.Updated, opts => opts.MapFrom(src => src.Updated.ToLocalTime()));

        CreateMap<ProductPartType, ProductPartTypeListDto>()
            .ForMember(dest => dest.ProductTypeName, opts => opts.MapFrom(src => src.ProductType.Title));

        CreateMap<ProductPartTypeCreateDto, ProductPartType>();

        CreateMap<ProductPartTypeUpdateDto, ProductPartType>()
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