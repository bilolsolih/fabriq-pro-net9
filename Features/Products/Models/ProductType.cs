﻿using System.Text.Json.Serialization;

namespace FabriqPro.Features.Products.Models;

public class ProductType
{
  public int Id { get; set; }
  public required string Title { get; set; }

  public ICollection<ProductModel> ProductModels { get; set; } = new List<ProductModel>();

  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }
}