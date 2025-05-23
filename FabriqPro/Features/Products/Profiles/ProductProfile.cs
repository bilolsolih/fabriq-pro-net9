﻿using AutoMapper;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models.Product;

namespace FabriqPro.Features.Products.Profiles;

public class ProductProfile : Profile
{
  public ProductProfile()
  {
    CreateMap<Product, ProductListDto>()
      .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.ProductType.Title))
      .ForMember(dest => dest.Model, opts => opts.MapFrom(src => src.ProductModel.Title))
      .ForMember(dest => dest.ColorTitle, opts => opts.MapFrom(src => src.ProductModel.Color.Title))
      .ForMember(dest => dest.ColorHex, opts => opts.MapFrom(src => src.ProductModel.Color.ColorCode));
    
    CreateMap<Product, ProductsAddedByMeListDto>()
      .ForMember(dest => dest.Master, opts => opts.MapFrom(src => $"{src.Master.FirstName} {src.Master.LastName}"))
      .ForMember(dest => dest.FromUser, opts => opts.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}"))
      .ForMember(dest => dest.ToUser, opts => opts.MapFrom(src => $"{src.ToUser.FirstName} {src.ToUser.LastName}"))
      .ForMember(dest => dest.ProductType, opts => opts.MapFrom(src => src.ProductType.Title))
      .ForMember(dest => dest.ProductModel, opts => opts.MapFrom(src => src.ProductModel.Title));

    CreateMap<Product, ProductsSentToMeListDto>()
      .ForMember(dest => dest.FromUser, opts => opts.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}"))
      .ForMember(dest => dest.ProductType, opts => opts.MapFrom(src => src.ProductType.Title))
      .ForMember(dest => dest.ProductModel, opts => opts.MapFrom(src => src.ProductModel.Title));
  }
}