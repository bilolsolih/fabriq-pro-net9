using AutoMapper;
using FabriqPro.Features.ProductParts.DTOs;
using FabriqPro.Features.ProductParts.Models;

namespace FabriqPro.Features.ProductParts.Profiles;

public class ProductPartToDepartmentProfiles : Profile
{
    public ProductPartToDepartmentProfiles()
    {
        CreateMap<ProductPartToDepartment, ProductPartToDepartmentDetailDto>()
            .ForMember(dest => dest.Created, opts => opts.MapFrom(src => src.Created.ToLocalTime()))
            .ForMember(dest => dest.Updated, opts => opts.MapFrom(src => src.Updated.ToLocalTime()));

        CreateMap<ProductPartToDepartment, ProductPartToDepartmentListDto>()
            .ForMember(dest => dest.ProductPartName, opts => opts.MapFrom(src => src.ProductPart.Title))
            .ForMember(dest => dest.DepartmentName, opts => opts.MapFrom(src => src.Department.Title));

        CreateMap<ProductPartToDepartmentCreateDto, ProductPartToDepartment>();

        CreateMap<ProductPartToDepartmentUpdateDto, ProductPartToDepartment>()
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