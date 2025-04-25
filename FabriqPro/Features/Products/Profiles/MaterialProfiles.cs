using AutoMapper;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Material;

namespace FabriqPro.Features.Products.Profiles;

public class MaterialProfiles : Profile
{
  public MaterialProfiles()
  {
    CreateMap<MaterialCreateDto, Material>();
    CreateMap<Material, MaterialTypeListDto>();
    CreateMap<MaterialToDepartment, MaterialListDto>()
      .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Material.Title))
      .ForMember(dest => dest.PartyNumber, opts => opts.MapFrom(src => src.Party.Title))
      .ForMember(dest => dest.Employee, opts => opts.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
      .ForMember(dest => dest.EmployeeRole, opts => opts.MapFrom(src => src.User.Role))
      .ForMember(dest => dest.Date, opts => opts.MapFrom(src => DateOnly.FromDateTime(src.Created)));
  }
}