using FabriqPro.Core;

namespace FabriqPro.Features.Products.Models.Product;

public class ProductType:BaseModel
{
  public required string Title { get; set; }
  public ICollection<ProductModel> ProductModels { get; set; } = [];
}