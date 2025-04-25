using FabriqPro.Core;
using FabriqPro.Features.Products.Models.Product;

namespace FabriqPro.Features.Products.Models;

public class Color : BaseModel
{
  public required string Title { get; set; }
  public required string ColorCode { get; set; }

  public ICollection<ProductModel> ProductModels { get; set; } = [];
}