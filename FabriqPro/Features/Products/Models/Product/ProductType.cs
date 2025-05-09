﻿namespace FabriqPro.Features.Products.Models.Product;

public class ProductType
{
  public int Id { get; set; }
  public required string Title { get; set; }

  public ICollection<ProductModel> ProductModels { get; set; } = [];

  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }
}