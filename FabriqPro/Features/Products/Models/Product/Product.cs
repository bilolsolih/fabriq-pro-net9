﻿using FabriqPro.Core;
using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Products.Models.Product;

public record Product : BaseModelRecord
{
  public int? OriginId { get; set; }
  public Product Origin { get; set; }
  
  public ICollection<Product> Transfers { get; set; } = [];
  
  public required Department Department { get; set; }
  
  public required int MasterId { get; set; }
  public User Master { get; set; }
  
  public int? FromUserId { get; set; }
  public User FromUser { get; set; }

  public int? ToUserId { get; set; }
  public User ToUser { get; set; }

  public required int ProductTypeId { get; set; }
  public ProductType ProductType { get; set; }

  public required int ProductModelId { get; set; }
  public ProductModel ProductModel { get; set; }

  public required int Quantity { get; set; }
  
  public required ItemStatus Status { get; set; }
}